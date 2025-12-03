using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;

namespace FlowTimer.Application.Services
{
    public class SessionService(ISessionRepository sessionRepository) : ISessionService
    {
        public event EventHandler<int>? SessionArchived;
        public event EventHandler<Session>? SessionCreated;
        public event EventHandler<Session>? SessionEdited;

        public async Task Archive(int id)
        {
            var result = await sessionRepository.Archive(id);

            if (result)
            {
                SessionArchived?.Invoke(this, id);
            }
        }

        public async Task CreateManual(int workItemId, DateTime startTime, DateTime endTime)
        {
            var session = new Session
            {
                WorkItemId = workItemId,
                StartTime = startTime,
                EndTime = endTime,
                IsManual = true
            };

            var result = await sessionRepository.Add(session);

            if (result)
            {
                SessionCreated?.Invoke(this, session);
            }
        }

        public async Task Edit(int id, DateTime startTime, DateTime endTime)
        {
            var session = await sessionRepository.GetById(id);
            if (session is null)
            {
                throw new InvalidOperationException($"Session with ID {id} not found.");
            }

            session.StartTime = startTime;
            session.EndTime = endTime;
            session.IsManual = true;

            var result = await sessionRepository.Update(session);

            if (result)
            {
                SessionEdited?.Invoke(this, session);
            }
        }

        public async Task<Session?> GetById(int id)
        {
            return await sessionRepository.GetById(id);
        }

        public async Task<List<Session>> GetByWorkItemId(int workItemId)
        {
            return await sessionRepository.GetByWorkItemId(workItemId);
        }
    }
}