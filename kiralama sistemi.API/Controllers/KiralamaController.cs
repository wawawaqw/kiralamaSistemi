using kiralama_sistemi.Entities.Enums;
using kiralamaSistemi.API.Model.Araba;
using kiralamaSistemi.API.Model.Kiralama;
using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace kiralamaSistemi.API.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class KiralamaController : ControllerBase
    {
        private readonly IRepository<Kiralama> _KiralamaRepository;
        private readonly IRepository<Araba> _arabaRepository;
        public KiralamaController(IRepository<Kiralama> kiralamaRepository,
                                  IRepository<Araba> arabaRepository)
        {
            _KiralamaRepository = kiralamaRepository;
            _arabaRepository = arabaRepository;
        }



        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetKiralamalar(ReqKiralama requst)
        {
            try
            {
                // Sorting
                Expression<Func<Kiralama, object>> expSort =
                    requst.SortColumn switch
                    {
                        "id" => i => i.ArabaId, 
                        _ => i => i.MusteriId,
                    };

                //Descending
                IOrderedQueryable<Kiralama> orderBy(IQueryable<Kiralama> i) => (requst.SortColumnDir == "ascend") ?
                                                                            i.OrderBy(expSort) :
                                                                            i.OrderByDescending(expSort);
                //Where
                Expression<Func<Kiralama, bool>> filter = i => true;



                //Selecting
                IQueryable<CreateUpdateKiralamaModel> select(IQueryable<Kiralama> i) => i.Select(i => new CreateUpdateKiralamaModel
                {

                    ArabaId = i.ArabaId,
                    MusteriId = i.MusteriId,
                    BaslangicTarihi = i.BaslangicTarihi,
                    SonTarihi = i.SonTarihi,
                    EnumKiralamaSuresi = i.EnumKiralamaSuresi

                });

                var (data, total) = await _KiralamaRepository.GetListAndCountAsync(select, filter, null, orderBy, requst.Skip, requst.PageSize);

                return Ok(new { total, data });
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(KiralamaController)} - {nameof(GetKiralama)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }



        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetKiralama(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }


                IQueryable<CreateUpdateKiralamaModel> select(IQueryable<Kiralama> i) => i.Select(i => new CreateUpdateKiralamaModel
                {
                    MusteriId = i.MusteriId,
                    ArabaId = i.ArabaId,
                    BaslangicTarihi = i.BaslangicTarihi,
                    SonTarihi = i.SonTarihi,
                    EnumKiralamaSuresi = i.EnumKiralamaSuresi

                });

                var entity = await _KiralamaRepository.FindAsync(select, i => i.MusteriId == User.GetUserId() && i.ArabaId == id)
                    ?? throw new OzelException(ErrorProvider.NotFoundData);

                return Ok(entity);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(KiralamaController)} - {nameof(GetKiralama)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }



        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateKiralama(CreateUpdateKiralamaModel model)
        {
            try
            {
                var entity = await _arabaRepository.FindAsync(i => i.Id == model.ArabaId && i.Durum)
                    ?? throw new OzelException(ErrorProvider.NotValidParams);


                entity.Durum = false;
                entity.Kiralamalar = new List<Kiralama>
                {
                    new Kiralama
                    {
                        MusteriId=User.GetUserId(),
                        EnumKiralamaSuresi=model.EnumKiralamaSuresi,
                        BaslangicTarihi = DateTime.Now,
                        SonTarihi = model.EnumKiralamaSuresi switch
                        {
                            EnumKiralamaSuresi.OnIki => DateTime.Now.AddHours(12),
                            EnumKiralamaSuresi.BirGun => DateTime.Now.AddDays(1),
                            EnumKiralamaSuresi.BirHafta => DateTime.Now.AddDays(7),
                            EnumKiralamaSuresi.BirAy => DateTime.Now.AddMonths(1),
                            _ => throw new OzelException(ErrorProvider.NotValidParams)
                        },
                    }

                };
                await _arabaRepository.UpdateAsync(entity);


                return Ok();
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(KiralamaController)} - {nameof(CreateKiralama)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> KiralamaUzat(CreateUpdateKiralamaModel model)
        {
            try
            {
                //var chek = await _arabaRepository.FindAsync(i => i.Id == model.ArabaId && !i.Durum,
                //    i => i.Include(i => i.Kiralamalar.Where(i => i.MusteriId == User.GetUserId())))
                //    ?? throw new OzelException(ErrorProvider.NotValidParams);

                var sonKiralama = await _KiralamaRepository.CustomAsync(async i => await i.OrderBy(i => i.BaslangicTarihi)
                .LastOrDefaultAsync(i => i.MusteriId == User.GetUserId() && i.ArabaId == model.ArabaId))
                    ?? throw new OzelException(ErrorProvider.NotValidParams);

                var sonTarih = sonKiralama.SonTarihi;

                var entity = new Kiralama
                {
                    MusteriId = User.GetUserId(),
                    ArabaId = model.ArabaId,
                    EnumKiralamaSuresi = model.EnumKiralamaSuresi,
                    BaslangicTarihi = sonTarih,
                    SonTarihi = model.EnumKiralamaSuresi switch
                    {
                        EnumKiralamaSuresi.OnIki => sonTarih.AddHours(12),
                        EnumKiralamaSuresi.BirGun => sonTarih.AddDays(1),
                        EnumKiralamaSuresi.BirHafta => sonTarih.AddDays(7),
                        EnumKiralamaSuresi.BirAy => sonTarih.AddMonths(1),
                        _ => throw new OzelException(ErrorProvider.NotValidParams)
                    },
                };

                await _KiralamaRepository.CreateAsync(entity);

                return Ok();
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(KiralamaController)} - {nameof(KiralamaUzat)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }




        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> UpdateKiralama(ResKiralama model)
        //{
        //    try
        //    {
        //        if (model.Id <= 0)
        //        {
        //            throw new OzelException(ErrorProvider.NotValidParams);
        //        }

        //        void action(Kiralama entity)
        //        {

        //            entity.ArabaId = model.ArabaId;
        //        }
        //        await _KiralamaRepository.UpdateAsync(action, i => i.Id == model.Id);
        //        return Ok(model);
        //    }
        //    catch (OzelException ex)
        //    {
        //        return BadRequest(ex.GetListError());
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(KiralamaController)} - {nameof(UpdateKiralama)}"));
        //        return BadRequest(ErrorProvider.APIHatasi);
        //    }
        //}


    }
}
