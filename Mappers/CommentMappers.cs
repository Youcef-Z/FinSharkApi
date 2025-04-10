using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
           return new CommentDto
           {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId
           };
        }

        public static Comment ToCreateCommentDto(this CreateCommentDto createCommentDto, int stockId)
        {
           return new  Comment
           {
                Title = createCommentDto.Title,
                Content = createCommentDto.Content,
                StockId = stockId
           };
        }

        public static Comment ToUpdateCommentDto(this UpdateCommentDto updateCommentDto)
        {
           return new  Comment
           {
                Title = updateCommentDto.Title,
                Content = updateCommentDto.Content,
           };
        }
    }
}