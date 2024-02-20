using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //It's good to have a string constant here
    [Route("api/comicbook")]
    [ApiController]
    public class ComicBookAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ComicBookAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                //return all the comic books from the database
                IEnumerable<ComicBook> objList = _db.ComicBooks.ToList();
                //_response.Result = objList;
                _response.Result = _mapper.Map<IEnumerable<ComicBookDTO>>(objList);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                ComicBook obj = _db.ComicBooks.First(u => u.ComicBookId == id);
                //_response.Result = obj;
                _response.Result = _mapper.Map<ComicBookDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByISBN/{isbn}")]
        public object GetByCode(string isbn)
        {
            try
            {
                ComicBook obj = _db.ComicBooks.First(u => u.ComicBookISBN.ToLower() == isbn.ToLower());
                //_response.Result = obj;
                _response.Result = _mapper.Map<ComicBookDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        public ResponseDTO Post([FromBody] ComicBookDTO couponDto)
        {
            try
            {
                ComicBook obj = _mapper.Map<ComicBook>(couponDto);
                _db.ComicBooks.Add(obj);
                _db.SaveChanges();


                _response.Result = _mapper.Map<ComicBookDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] ComicBookDTO couponDto)
        {
            try
            {
                ComicBook obj = _mapper.Map<ComicBook>(couponDto);
                _db.ComicBooks.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ComicBookDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                ComicBook obj = _db.ComicBooks.First(u => u.ComicBookId == id);
                _db.ComicBooks.Remove(obj);
                _db.SaveChanges();


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
