using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;
using Microsoft.AspNetCore.Components.Web;

namespace api.Mappers
{
    //converting a model to a particular dto 
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment CommentModel ){
            return new CommentDto{
                Id=CommentModel.Id,
                Title=CommentModel.Title,
                Content=CommentModel.Content,
                CreatedOn= CommentModel.CreatedOn,
                CreatedBy=CommentModel.AppUser.UserName,
                StockId=CommentModel.StockId

            };

        }
         public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId ){
            return new Comment{
                
                Title=commentDto.Title,
                Content=commentDto.Content,
                StockId=stockId
            };

        }
         public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto){
            return new Comment{
                
                Title=commentDto.Title,
                Content=commentDto.Content,
       
            };

        }
    }
}