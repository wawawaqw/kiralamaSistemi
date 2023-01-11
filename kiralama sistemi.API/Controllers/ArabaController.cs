using kiralamaSistemi.API.Model.Araba;
using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Tables;
using LinqKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace kiralamaSistemi.API.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ArabaController : ControllerBase
    {

        private readonly IRepository<Araba> _arabaRepository;
        public ArabaController(IRepository<Araba> arabaRepository)
        {
            _arabaRepository = arabaRepository;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetArabalar(ReqAraba requst)
            {
                try
                {
                    // Sorting
                    Expression<Func<Araba, object>> expSort =
                        requst.SortColumn switch
                        { 
                            "id" => i => i.Id,
                            _ => i => i.Id,
                        };

                    //Descending
                    IOrderedQueryable<Araba> orderBy(IQueryable<Araba> i) => (requst.SortColumnDir == "ascend") ?
                                                                                i.OrderBy(expSort) :
                                                                                i.OrderByDescending(expSort);
                    //Where
                    Expression<Func<Araba, bool>> filter = i => true;

                //Search 
                if (!string.IsNullOrEmpty(requst.Search))
                {
                    filter = filter.And(i => i.ArabaNumrasi.Contains(requst.Search)
                                          || i.Model.Contains(requst.Search));
                }


                //Selecting
                IQueryable<CreateUpdateArabaModel> select(IQueryable<Araba> i) => i.Select(i => new CreateUpdateArabaModel
                    {
                        Id = i.Id,
                        Model = i.Model,
                        Durum= i.Durum,
                        ArabaNumrasi = i.ArabaNumrasi,
                        EnumArabaRenk = i.EnumArabaRenk,
                        EnumArabaTur = i.EnumArabaTur,
                        MusteriId= i.MusteriId, 
                        Notlar = i.Notlar,

                    });

                    var (data, total) = await _arabaRepository.GetListAndCountAsync(select, filter, null, orderBy, requst.Skip, requst.PageSize);

                    return Ok(new { total, data });
                }
                catch (OzelException ex)
                {
                    return BadRequest(ex.GetListError());
                }
                catch (Exception ex)
                {
                    ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(GetArabalar)}"));
                    return BadRequest(ErrorProvider.APIHatasi);
                }
            }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAraba(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }


                IQueryable<CreateUpdateArabaModel> select(IQueryable<Araba> i) => i.Select(i => new CreateUpdateArabaModel  
                {
                    Id = i.Id,
                    Durum= i.Durum,
                    Model = i.Model,
                    ArabaNumrasi = i.ArabaNumrasi,
                    EnumArabaRenk = i.EnumArabaRenk,
                    EnumArabaTur = i.EnumArabaTur,
                    Notlar = i.Notlar,
                    MusteriId=i.MusteriId,
                    Tarifeler=i.Tarifalar.Select(i=> new TarifeModel()
                    {
                        TarifTutar = i.TarifTutar,
                        EnumKiralamaSuresi = i.EnumKiralamaSuresi,

                    }).ToList(),

                    //Kiralamalar = i.Kiralamalar.Select(i => new KiralamaModel
                    //{
                    //    KiralamaTarihi = i.KiralamaTarihi,
                    //}).ToList(),

                });

                var entity = await _arabaRepository.FindAsync(select, i => i.Id == id)
                    ?? throw new OzelException(ErrorProvider.NotFoundData);

                return Ok(entity);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(GetAraba)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetArabalarByMusteri(ReqAraba requst)
        {
            try
            {
                // Sorting
                Expression<Func<Araba, object>> expSort =
                    requst.SortColumn switch
                    {
                        "id" => i => i.Id,
                        _ => i => i.Id,
                    };

                //Descending
                IOrderedQueryable<Araba> orderBy(IQueryable<Araba> i) => (requst.SortColumnDir == "ascend") ?
                                                                            i.OrderBy(expSort) :
                                                                            i.OrderByDescending(expSort);
                //Where
                Expression<Func<Araba, bool>> filter = i => i.MusteriId==User.GetUserId();

                //Search 
                if (!string.IsNullOrEmpty(requst.Search))
                {
                    filter = filter.And(i => i.ArabaNumrasi.Contains(requst.Search)
                                          || i.Model.Contains(requst.Search));
                }


                //Selecting
                IQueryable<CreateUpdateArabaModel> select(IQueryable<Araba> i) => i.Select(i => new CreateUpdateArabaModel
                {
                    Id = i.Id,
                    Model = i.Model,
                    Durum= i.Durum,
                    ArabaNumrasi = i.ArabaNumrasi,
                    EnumArabaRenk = i.EnumArabaRenk,
                    EnumArabaTur = i.EnumArabaTur,
                    MusteriId = i.MusteriId,
                    Notlar = i.Notlar,

                    //Tarifeler = i.Tarifalar.Select(i => new TarifeModel
                    //{
                    //    EnumKiralamaSuresi= i.EnumKiralamaSuresi
                    //    TarifTutar = i.TarifTutar,
                    //}).ToList(),

                    //Kiralamalar = i.Kiralamalar.Select(i => new KiralamaModel
                    //{
                    //    KiralamaTarihi = i.KiralamaTarihi,
                    //}).ToList(),
                });

                var (data, total) = await _arabaRepository.GetListAndCountAsync(select, filter, null, orderBy, requst.Skip, requst.PageSize);

                return Ok(new { total, data });
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(GetArabalarByMusteri)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetArabaByMusteri(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }


                IQueryable<CreateUpdateArabaModel> select(IQueryable<Araba> i) => i.Select(i => new CreateUpdateArabaModel
                {
                    Id = i.Id,
                    Model = i.Model,
                    Durum= i.Durum,
                    ArabaNumrasi = i.ArabaNumrasi,
                    EnumArabaRenk = i.EnumArabaRenk,
                    EnumArabaTur = i.EnumArabaTur,
                    Notlar = i.Notlar,
                    Tarifeler = i.Tarifalar.Select(i => new TarifeModel()
                    {
                        TarifTutar = i.TarifTutar,
                        EnumKiralamaSuresi = i.EnumKiralamaSuresi,

                    }).ToList(),

                    //Kiralamalar = i.Kiralamalar.Select(i => new KiralamaModel
                    //{
                    //    KiralamaTarihi = i.KiralamaTarihi,
                    //}).ToList(),

                });

                var entity = await _arabaRepository.FindAsync(select, i => i.MusteriId == id)
                    ?? throw new OzelException(ErrorProvider.NotFoundData);

                return Ok(entity);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(GetArabaByMusteri)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateAraba(CreateUpdateArabaModel model)
        {
            try
            {
                var entity = new Araba
                {
                    ArabaNumrasi = model.ArabaNumrasi,
                    EnumArabaRenk = model.EnumArabaRenk,
                    Model = model.Model,
                    Durum=model.Durum,
                    Notlar = model.Notlar,
                    MusteriId=User.GetUserId(),
                    EnumArabaTur = model.EnumArabaTur,
                    Tarifalar = model.Tarifeler.Select(i => new Tarife()
                    {
                        EnumKiralamaSuresi = i.EnumKiralamaSuresi,
                        TarifTutar = i.TarifTutar,
                    }).ToList(),
                    //Kiralamalar = model.Kiralama.Select(i => new Kiralama()
                    //{
                    //    MusteriId = 2,
                    //MusteriId=User.GetUserId(),
                    //    KiralamaTarihi = DateTime.Now,
                    //    //MusteriId = User.GetUserId(),
                    //}).ToList(),
                };

                await _arabaRepository.CreateAsync(entity);

                return Ok(entity.Id);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(CreateAraba)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateAraba(ResAraba model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }

                void action(Araba entity)
                {
                    entity.Id = model.Id;
                    entity.Durum=model.Durum;
                    entity.ArabaNumrasi= model.ArabaNumrasi;
                    entity.EnumArabaTur= model.EnumArabaTur;
                    entity.EnumArabaRenk= model.EnumArabaRenk;
                    entity.Notlar= model.Notlar;
                    entity.Model = model.Model;
                }
                await _arabaRepository.UpdateAsync(action, i => i.Id == model.Id);
                return Ok(model);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(UpdateAraba)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpDelete]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAraba(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }

                await _arabaRepository.DeleteAsync(i => i.Id == id);
                return Ok();
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(DeleteAraba)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }
        /*
         var entity = new Araba
                {
                    ArabaNumrasi = model.ArabaNumrasi,
                    EnumArabaRenk = model.EnumArabaRenk,
                    Model = model.Model,
                    Notlar = model.Notlar,
                    MusteriId=2,
                    EnumArabaTur = model.EnumArabaTur,
                    Tarifalar = model.Tarifeler.Select(i => new Tarife()
                    {
                        EnumKiralamaSuresi = i.EnumKiralamaSuresi,
                        TarifTutar = i.TarifTutar,
                    }).ToList(),
                    Kiralamalar = model.Kiralama.Select(i => new Kiralama()
                    {
                        MusteriId = 2,
                        KiralamaTarihi = DateTime.Now,
                        //MusteriId = User.GetUserId(),
                    }).ToList(),
                };

                await _arabaRepository.CreateAsync(entity);
         */

    }
}
