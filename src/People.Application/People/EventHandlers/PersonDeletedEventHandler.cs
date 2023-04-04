using MediatR;
using Microsoft.Extensions.Caching.Memory;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.EventHandlers
{
    internal class PersonDeletedEventHandler : INotificationHandler<PersonDeletedEvent>
    {
        private readonly IMemoryCache _memoryCache;

        public PersonDeletedEventHandler(IMemoryCache cache)
        {
            _memoryCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task Handle(PersonDeletedEvent notification, CancellationToken cancellationToken)
        {
            string cacheKey = $"Person-{notification.Person.Id}";
            _memoryCache.Remove(cacheKey);
            return Task.CompletedTask;
        }
    }

    class PersonDeletedEvent : INotification
    {
        public Person Person { get; set; }
    }
}