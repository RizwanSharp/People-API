using MediatR;
using Microsoft.Extensions.Caching.Memory;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.EventHandlers
{
    internal class PersonCreatedOrUpdatedEventHandler : INotificationHandler<PersonCreatedOrUpdatedEvent>
    {
        private readonly IMemoryCache _memoryCache;

        public PersonCreatedOrUpdatedEventHandler(IMemoryCache cache)
        {
            _memoryCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task Handle(PersonCreatedOrUpdatedEvent notification, CancellationToken cancellationToken)
        {
            string cacheKey = $"Person-{notification.Person.Id}";
            _memoryCache.Set(cacheKey, notification.Person);
            return Task.CompletedTask;
        }
    }

    class PersonCreatedOrUpdatedEvent : INotification
    {
        public Person Person { get; set; }
    }
}