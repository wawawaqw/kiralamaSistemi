using kiralamaSistemi.API.Model.Araba;
using kiralamaSistemi.API.Model.Tarife;
using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Extensions;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace kiralamaSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifeController : ControllerBase
    {


        private readonly IRepository<Tarife> _TarifeRepository;
        public TarifeController(IRepository<Tarife> TarifeRepository)
        {
            _TarifeRepository = TarifeRepository;
        }



        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetTarifeler(ReqTarife requst)
        {
            try
            {
                // Sorting
                Expression<Func<Tarife, object>> expSort =
                    requst.SortColumn switch
                    {
                        "id" => i => i.Id,
                        _ => i => i.Id,
                    };

                //Descending
                IOrderedQueryable<Tarife> orderBy(IQueryable<Tarife> i) => (requst.SortColumnDir == "ascend") ?
                                                                            i.OrderBy(expSort) :
                                                                            i.OrderByDescending(expSort);
                //Where
                Expression<Func<Tarife, bool>> filter = i => true;

                ////Search 
                //if (!string.IsNullOrEmpty(requst.Search))
                //{
                //    filter = filter.And(i => i.ArabaNumrasi.Contains(requst.Search)
                //                          || i.Model.Contains(requst.Search));
                //}


                //Selecting
                IQueryable<CreateUpdateTarifeModel> select(IQueryable<Tarife> i) => i.Select(i => new CreateUpdateTarifeModel
                {
                    Id = i.Id,
                    ArabaId= i.ArabaId,
                    Model=i.Araba.Model,
                    EnumKiralamaSuresi=i.EnumKiralamaSuresi,
                    TarifTutar=i.TarifTutar,
                });

                var (data, total) = await _TarifeRepository.GetListAndCountAsync(select, filter, null, orderBy, requst.Skip, requst.PageSize);

                return Ok(new { total, data });
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(TarifeController)} - {nameof(GetTarifeler)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }




        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetTarife(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }


                IQueryable<CreateUpdateTarifeModel> select(IQueryable<Tarife> i) => i.Select(i => new CreateUpdateTarifeModel
                {
                    Id = i.Id,
                    TarifTutar = i.TarifTutar,
                    EnumKiralamaSuresi = i.EnumKiralamaSuresi,
                    ArabaId=i.ArabaId,
                    Model = i.Araba.Model,
                });

                var entity = await _TarifeRepository.FindAsync(select, i => i.Id == id)
                    ?? throw new OzelException(ErrorProvider.NotFoundData);

                return Ok(entity);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(ArabaController)} - {nameof(GetTarife)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }




        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateTarife(CreateUpdateTarifeModel model)
        {
            try
            {
                if(model.EnumKiralamaSuresi.TryToString().TryToInt() <= 0 || model.EnumKiralamaSuresi.TryToString().TryToInt() > 4) 
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }
                var entity = new Tarife
                {
                    ArabaId= model.ArabaId,
                    EnumKiralamaSuresi= model.EnumKiralamaSuresi,
                    TarifTutar= model.TarifTutar,
                };

                await _TarifeRepository.CreateAsync(entity);

                return Ok(entity.Id);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(TarifeController)} - {nameof(CreateTarife)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }




        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateTarife(ResTarife model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }

                void action(Tarife entity)
                {
                    entity.Id = model.Id;
                    entity.TarifTutar = model.TarifTutar;
                    entity.EnumKiralamaSuresi=model.EnumKiralamaSuresi;
                }
                await _TarifeRepository.UpdateAsync(action, i => i.Id == model.Id);
                return Ok(model);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(TarifeController)} - {nameof(UpdateTarife)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }



        [HttpDelete]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeleteTarife(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }

                await _TarifeRepository.DeleteAsync(i => i.Id == id);
                return Ok();
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(API)} - {nameof(Controllers)} - {nameof(TarifeController)} - {nameof(DeleteTarife)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }



    }
}
