﻿using MediatR;
using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryCommentsViewModel>>
    {
        public GetEntryCommentsQuery(Guid entryId, Guid? userId, int page, int pageSize) : base(page, pageSize)
        {
            EntryId = entryId;
            UserId = userId;
        }

        public Guid EntryId { get; set; }
        public Guid? UserId { get; set; }
    }

}
