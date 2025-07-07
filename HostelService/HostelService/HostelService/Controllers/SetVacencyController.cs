using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.LotteryModel;
using Service.Interface;
using System.Collections.Generic;

namespace HostelService.Controllers
{
    public class SetVacencyController : APIBaseController
    {
        private readonly ISetVacencyService vacencyService;
        private readonly IHostelProfileService hostelProfileService;

        public SetVacencyController(ISetVacencyService vacencyService, IHostelProfileService hostelProfileService)
        {
            this.vacencyService = vacencyService;
            this.hostelProfileService = hostelProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int TypeOfCourseId)
        {
            return Ok(await vacencyService.FetchVacancyListAsync(TypeOfCourseId));
        }

        [HttpGet("get-quota-type-vacancy")]
        public async Task<IActionResult> GetQuotaType()
        {
            return Ok(await vacencyService.FetchQuotaTypeAsync());
        }

        [HttpPost("set-quota-type-vacancy")]
        public async Task<IActionResult> PostSetQuotaTypeVancies([FromBody] QuotaTypeVacancyModel model)
        {
            var resultFetchVacancy = await vacencyService.FetchQuotaTypeAsync();

            var cellingSpecial = 0;
            var floorSpecial = 0;

            var cellingCourseType = 0;
            var floorCourseType = 0;

            if (resultFetchVacancy != null)
            {
                cellingSpecial = (int)Math.Ceiling((decimal)resultFetchVacancy[0].Calculated_Vacancy);
                floorSpecial = (int)Math.Floor((decimal)resultFetchVacancy[0].Calculated_Vacancy);

                cellingCourseType = (int)Math.Ceiling((decimal)resultFetchVacancy[1].Calculated_Vacancy);
                floorCourseType = (int)Math.Floor((decimal)resultFetchVacancy[1].Calculated_Vacancy);
            }

            var obj = new QuotaTypeObjectClass
            {
                SpecialQuota_Vacancy_setby_warden = model.QuotaTypeVacancies.Where(x => x.Id == 1).Select(x => x.Vacancy).FirstOrDefault(),
                CourseQuota_Vacancy_setby_Warden = model.QuotaTypeVacancies.Where(x => x.Id == 2).Select(x => x.Vacancy).FirstOrDefault(),
            };

            if ((cellingSpecial == obj.SpecialQuota_Vacancy_setby_warden || floorSpecial == obj.SpecialQuota_Vacancy_setby_warden) &&
                (cellingCourseType == obj.CourseQuota_Vacancy_setby_Warden || floorCourseType == obj.CourseQuota_Vacancy_setby_Warden))
            {

            }
            else
                return Ok(new { Status = false, Message = "Calculated and Entered Vacancies do not match" });

            var result = await vacencyService.InsertQuotaTypeVacancyAsync(obj);

            if (result != null && result == "1")
                return Ok(new { Status = true, Message = "Vacancies set Successfully." });
            else if (result != null && result == "2")
                return Ok(new { Status = false, Message = "Vacancies could not be set. Please try again" });
            else if (result != null && result == "3")
                return Ok(new { Status = false, Message = "Record for your hostel could not be found. Please contact Administrator" });
            else
                return Ok(new { Status = false, Message = "Some error occurred. Please try again" });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VacencySetModel model)
        {
            var List = await vacencyService.FetchVacancyListAsync(model.CourseTypeId);

            if (model.Vacancies.Count == model.Vacancies.Count())
            {
                foreach (var item in model.Vacancies)
                {
                    var Data = List?.Where(x => x.CasteCategoryVacancyId == item.Id).FirstOrDefault();

                    if ((int)Math.Floor((decimal)Data.Calculated_Vacancy) == item.Vacancy || (int)Math.Ceiling((decimal)Data.Calculated_Vacancy) == item.Vacancy)
                    {

                    }
                    else
                        return Ok(new { Status = false, Message = $"Round of {Data.Calculated_Vacancy} and {item.Vacancy} not matching." });

                    var subCasteFromList = List?.Where(a => a.CasteCategoryVacancyId == item.Id).ToList();

                    var subCasteListModel = subCasteFromList?.Select(y => y.VacancyListCasteWise).ToList();

                    if (subCasteListModel[0].Count > 0)
                    {
                        foreach (var subCasteItem in item.VacancyListCasteWise)
                        {
                            var dataSubCasteVacancy = subCasteListModel[0]?.Where(a => a.CasteVacancyId == subCasteItem.CasteVacancyId).FirstOrDefault();

                            if ((int)Math.Floor((decimal)dataSubCasteVacancy.Caste_Calculated_Vacancy) == subCasteItem.CasteVacancy || (int)Math.Ceiling((decimal)(dataSubCasteVacancy.Caste_Calculated_Vacancy)) == subCasteItem.CasteVacancy)
                            {

                            }
                            else
                                return Ok(new { Status = false, Message = $"Round of {dataSubCasteVacancy.Caste_Calculated_Vacancy} and {subCasteItem.CasteVacancy} not matching." });

                        }

                        var cellingTotalSubCaste = (int)Math.Ceiling((decimal)subCasteFromList[0].VacancyListCasteWise.Sum(x => x.Caste_Calculated_Vacancy));
                        var floorTotalSubCaste = (int)Math.Floor((decimal)subCasteFromList[0].VacancyListCasteWise.Sum(x => x.Caste_Calculated_Vacancy));
                        var totaSubCaste = item.VacancyListCasteWise.Sum(x => x.CasteVacancy);
                        if ((cellingTotalSubCaste == totaSubCaste) || (floorTotalSubCaste == totaSubCaste))
                        {

                        }
                        else
                            return Ok(new { Status = false, Message = $"Caste Vacancies sum not matched." });

                    }


                }



                var cellingTotal = (int)Math.Ceiling((decimal)List.Sum(x => x.Calculated_Vacancy));
                var floorTotal = (int)Math.Floor((decimal)List.Sum(x => x.Calculated_Vacancy));
                var total = model.Vacancies.Sum(x => x.Vacancy);
                if ((cellingTotal == total) || (floorTotal == total))
                {
                    await vacencyService.UpsertVacancyAsync(model);
                    return Ok(new { Status = true, Message = $"Vacancy set successfully." });
                }
                else
                    return Ok(new { Status = false, Message = $"Caste Categories Vacancy sum not matched." });
            }
            else
                return Ok(new { Status = false, Message = $"Please set vacancy for all category." });
        }

        [HttpGet("get-all-quota-vacancies")]
        public async Task<IActionResult> GetAllQuotaVacancies()
        {
            return Ok(await vacencyService.FetchAllQuotaDropDownAsync());
        }
        [HttpGet("fetch-course-type-vacancies")]
        public async Task<IActionResult> GetCourseTypeVacancies()
        {
            return Ok(await vacencyService.FetchCourseTypeAsync());
        }
        [HttpPost("set-coursetype-vacancies")]
        public async Task<IActionResult> PostCourseTypeVacancies([FromBody] CourseTypeSetVacancyModel model)
        {
            var List = await vacencyService.FetchCourseTypeAsync();

            if (model.CourseTypeVacancies.Count == model.CourseTypeVacancies.Count())
            {
                foreach (var item in model.CourseTypeVacancies)
                {
                    var Data = List.CourseTypeVacancyList?.Where(x => x.Id == item.Id).FirstOrDefault();

                    if ((int)Math.Floor((decimal)Data.Calculated_Vacancy) == item.Vacancy || (int)Math.Ceiling((decimal)Data.Calculated_Vacancy) == item.Vacancy)
                    {

                    }
                    else
                        return Ok(new { Status = false, Message = $"Round of {Data.Calculated_Vacancy} and {item.Vacancy} not matching." });
                }

                var cellingTotal = (int)Math.Ceiling((decimal)List.CourseTypeVacancyList.Sum(x => x.Calculated_Vacancy));
                var floorTotal = (int)Math.Floor((decimal)List.CourseTypeVacancyList.Sum(x => x.Calculated_Vacancy));
                var total = model.CourseTypeVacancies.Sum(x => x.Vacancy);
                if ((cellingTotal == total) || (floorTotal == total))
                {
                    await vacencyService.UpsertCourseTypeVacancyAsync(model);
                    return Ok(new { Status = true, Message = $"Vacancy set successfully." });
                }
                else
                    return Ok(new { Status = false, Message = $"Course Type Vacancy sum not matched." });
            }
            else
                return Ok(new { Status = false, Message = $"Please set vacancies for all course types." });
        }

        [HttpGet("check-if-capacities-set")]
        public async Task<IActionResult> CheckIfCapacitiesSet()
        {
            return Ok(await vacencyService.CheckIfCapacitiesSetAsync());
        }


        [HttpGet("check-if-capacities-set-vjnt")]
        public async Task<IActionResult> CheckIfCapacitiesSetVjnt(int hostelId)
        {
            return Ok(await vacencyService.CheckIfCapacitiesSetAsyncVjnt(hostelId));
        }

        [HttpGet("get-quota-type-vacancy-vjnt")]
        public async Task<IActionResult> GetQuotaTypeVjnt(int hostelId)
        {
            return Ok(await vacencyService.FetchQuotaTypeAsyncVjnt(hostelId));
        }

        [HttpGet("fetch-course-type-vacancies-vjnt")]
        public async Task<IActionResult> GetCourseTypeVacanciesVjnt(int hostelId)
        {
            return Ok(await vacencyService.FetchCourseTypeAsyncVjnt(hostelId));
        }

        [HttpGet("view-vacancy-vjnt")]
        public async Task<IActionResult> GetViewVacancyVjnt(int hostelId,  int TypeOfCourseId)
        {
            return Ok(await vacencyService.FetchVacancyListAsyncVjnt( hostelId,TypeOfCourseId));
        }

        [HttpPost("set-quota-type-vacancy-vjnt")]
        public async Task<IActionResult> PostSetQuotaTypeVanciesVjnt( [FromBody] QuotaTypeVacancyModel model)
        {
            var resultFetchVacancy = await vacencyService.FetchQuotaTypeAsyncVjnt(model.HostelId);

            var cellingSpecial = 0;
            var floorSpecial = 0;

            var cellingCourseType = 0;
            var floorCourseType = 0;

            if (resultFetchVacancy != null)
            {
                cellingSpecial = (int)Math.Ceiling((decimal)resultFetchVacancy[0].Calculated_Vacancy);
                floorSpecial = (int)Math.Floor((decimal)resultFetchVacancy[0].Calculated_Vacancy);

                cellingCourseType = (int)Math.Ceiling((decimal)resultFetchVacancy[1].Calculated_Vacancy);
                floorCourseType = (int)Math.Floor((decimal)resultFetchVacancy[1].Calculated_Vacancy);
            }

            var obj = new QuotaTypeObjectClass
            {
                SpecialQuota_Vacancy_setby_warden = model.QuotaTypeVacancies.Where(x => x.Id == 1).Select(x => x.Vacancy).FirstOrDefault(),
                CourseQuota_Vacancy_setby_Warden = model.QuotaTypeVacancies.Where(x => x.Id == 2).Select(x => x.Vacancy).FirstOrDefault(),
            };

            if ((cellingSpecial == obj.SpecialQuota_Vacancy_setby_warden || floorSpecial == obj.SpecialQuota_Vacancy_setby_warden) &&
                (cellingCourseType == obj.CourseQuota_Vacancy_setby_Warden || floorCourseType == obj.CourseQuota_Vacancy_setby_Warden))
            {

            }
            else
                return Ok(new { Status = false, Message = "Calculated and Entered Vacancies do not match" });

            var result = await vacencyService.InsertQuotaTypeVacancyAsyncVjnt(model.HostelId, obj);

            if (result != null && result == "1")
                return Ok(new { Status = true, Message = "Vacancies set Successfully." });
            else if (result != null && result == "2")
                return Ok(new { Status = false, Message = "Vacancies could not be set. Please try again" });
            else if (result != null && result == "3")
                return Ok(new { Status = false, Message = "Record for your hostel could not be found. Please contact Administrator" });
            else
                return Ok(new { Status = false, Message = "Some error occurred. Please try again" });
        }

        [HttpPost("set-coursetype-vacancies-vjnt")]
        public async Task<IActionResult> PostCourseTypeVacanciesVjnt([FromBody] CourseTypeSetVacancyModel model)
        {
            var List = await vacencyService.FetchCourseTypeAsyncVjnt(model.HostelId);

            if (model.CourseTypeVacancies.Count == model.CourseTypeVacancies.Count())
            {
                foreach (var item in model.CourseTypeVacancies)
                {
                    var Data = List.CourseTypeVacancyList?.Where(x => x.Id == item.Id).FirstOrDefault();

                    if ((int)Math.Floor((decimal)Data.Calculated_Vacancy) == item.Vacancy || (int)Math.Ceiling((decimal)Data.Calculated_Vacancy) == item.Vacancy)
                    {

                    }
                    else
                        return Ok(new { Status = false, Message = $"Round of {Data.Calculated_Vacancy} and {item.Vacancy} not matching." });
                }

                var cellingTotal = (int)Math.Ceiling((decimal)List.CourseTypeVacancyList.Sum(x => x.Calculated_Vacancy));
                var floorTotal = (int)Math.Floor((decimal)List.CourseTypeVacancyList.Sum(x => x.Calculated_Vacancy));
                var total = model.CourseTypeVacancies.Sum(x => x.Vacancy);
                if ((cellingTotal == total) || (floorTotal == total))
                {
                    await vacencyService.UpsertCourseTypeVacancyAsync(model);
                    return Ok(new { Status = true, Message = $"Vacancy set successfully." });
                }
                else
                    return Ok(new { Status = false, Message = $"Course Type Vacancy sum not matched." });
            }
            else
                return Ok(new { Status = false, Message = $"Please set vacancies for all course types." });
        }

        [HttpPost("setvacancy_vjnt")]
        public async Task<IActionResult> PostVjnt([FromBody] VacencySetModel model)
        {
            var List = await vacencyService.FetchVacancyListAsyncVjnt(model.HostelId, model.CourseTypeId);  // FetchVacancyListAsyncVjnt(hostelId, TypeOfCourseId)

            if (model.Vacancies.Count == model.Vacancies.Count())
            {
                foreach (var item in model.Vacancies)
                {
                    var Data = List?.Where(x => x.CasteCategoryVacancyId == item.Id).FirstOrDefault();

                    if ((int)Math.Floor((decimal)Data.Calculated_Vacancy) == item.Vacancy || (int)Math.Ceiling((decimal)Data.Calculated_Vacancy) == item.Vacancy)
                    {

                    }
                    else
                        return Ok(new { Status = false, Message = $"Round of {Data.Calculated_Vacancy} and {item.Vacancy} not matching." });

                    var subCasteFromList = List?.Where(a => a.CasteCategoryVacancyId == item.Id).ToList();

                    var subCasteListModel = subCasteFromList?.Select(y => y.VacancyListCasteWise).ToList();

                    if (subCasteListModel[0].Count > 0)
                    {
                        foreach (var subCasteItem in item.VacancyListCasteWise)
                        {
                            var dataSubCasteVacancy = subCasteListModel[0]?.Where(a => a.CasteVacancyId == subCasteItem.CasteVacancyId).FirstOrDefault();

                            if ((int)Math.Floor((decimal)dataSubCasteVacancy.Caste_Calculated_Vacancy) == subCasteItem.CasteVacancy || (int)Math.Ceiling((decimal)(dataSubCasteVacancy.Caste_Calculated_Vacancy)) == subCasteItem.CasteVacancy)
                            {

                            }
                            else
                                return Ok(new { Status = false, Message = $"Round of {dataSubCasteVacancy.Caste_Calculated_Vacancy} and {subCasteItem.CasteVacancy} not matching." });

                        }

                        var cellingTotalSubCaste = (int)Math.Ceiling((decimal)subCasteFromList[0].VacancyListCasteWise.Sum(x => x.Caste_Calculated_Vacancy));
                        var floorTotalSubCaste = (int)Math.Floor((decimal)subCasteFromList[0].VacancyListCasteWise.Sum(x => x.Caste_Calculated_Vacancy));
                        var totaSubCaste = item.VacancyListCasteWise.Sum(x => x.CasteVacancy);
                        if ((cellingTotalSubCaste == totaSubCaste) || (floorTotalSubCaste == totaSubCaste))
                        {

                        }
                        else
                            return Ok(new { Status = false, Message = $"Caste Vacancies sum not matched." });

                    }


                }



                var cellingTotal = (int)Math.Ceiling((decimal)List.Sum(x => x.Calculated_Vacancy));
                var floorTotal = (int)Math.Floor((decimal)List.Sum(x => x.Calculated_Vacancy));
                var total = model.Vacancies.Sum(x => x.Vacancy);
                if ((cellingTotal == total) || (floorTotal == total))
                {
                    await vacencyService.UpsertVacancyAsyncVjnt(model);
                    return Ok(new { Status = true, Message = $"Vacancy set successfully." });
                }
                else
                    return Ok(new { Status = false, Message = $"Caste Categories Vacancy sum not matched." });
            }
            else
                return Ok(new { Status = false, Message = $"Please set vacancy for all category." });
        }

    }
}
