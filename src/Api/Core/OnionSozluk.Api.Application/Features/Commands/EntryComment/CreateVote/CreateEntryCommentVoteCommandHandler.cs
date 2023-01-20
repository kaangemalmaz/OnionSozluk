using MediatR;
using OnionSozluk.Common;
using OnionSozluk.Common.Events.EntryComment;
using OnionSozluk.Common.Infrastructure;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Features.Commands.EntryComment.CreateVote
{
    public class CreateEntryCommentVoteCommandHandler :
    IRequestHandler<CreateEntryCommentVoteCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryCommentVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.VoteExchangeName,
                exchangeType: SozlukConstants.DefaultExchangeType,
                queueName: SozlukConstants.CreateEntryCommentVoteQueueName,
                obj: new CreateEntryCommentVoteEvent()
                {
                    EntryCommentId = request.EntryCommentId,
                    VoteType = request.VoteType,
                    CreatedBy = request.CreatedBy
                });

            return await Task.FromResult(true);
        }
    }
}
