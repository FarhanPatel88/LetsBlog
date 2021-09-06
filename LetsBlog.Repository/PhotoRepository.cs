﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LetsBlog.Models.Photo;
using Microsoft.Extensions.Configuration;

namespace LetsBlog.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IConfiguration _config;

        public PhotoRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<int> DeleteAsync(int photoId)
        {
            int affectedrows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedrows = await connection.ExecuteAsync("Photo_Delete", new { PhotoId = photoId }, commandType: CommandType.StoredProcedure);
            }

            return affectedrows;
        }

        public async Task<Photo> GetAsync(int photoId)
        {
            Photo photo;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photo = await connection.QueryFirstOrDefaultAsync<Photo>("Photo_Get", new { PhotoId = photoId }, commandType: CommandType.StoredProcedure);
            }

            return photo;
        }

        public async Task<List<Photo>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Photo> photos;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photos = await connection.QueryAsync<Photo>("Photo_GetByUserId", new { ApplicationUserId = applicationUserId }, commandType: CommandType.StoredProcedure);
            }

            return photos.ToList();
        }

        public async Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId)
        {
            Photo photo;
            var dataTable = new DataTable();
            dataTable.Columns.Add("PublicId", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            dataTable.Rows.Add(photoCreate.PublicId, photoCreate.ImageUrl, photoCreate.Description);

            int newPhotoId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newPhotoId = await connection.ExecuteScalarAsync<int>("Photo_Insert", new { Photo = dataTable.AsTableValuedParameter("dbo.PhotoType") }, commandType: CommandType.StoredProcedure);
            }

            photo = await GetAsync(newPhotoId);

            return photo;

        }
    }
}
