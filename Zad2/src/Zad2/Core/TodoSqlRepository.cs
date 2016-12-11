using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace MatijaClassLibrary
{
    class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);
            if (!item.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User that is trying to access data isn't the owner.");
            return item;
        }

        public void Add(TodoItem todoItem)
        {
            if(_context.TodoItems.Find(todoItem.Id) != null)
                throw new DuplicateTodoItemException("Item already exists.");
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);
            if (item == null)
                return false;
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var item = _context.TodoItems.Find(todoItem.Id);
            if (item == null)
            {
                this.Add(todoItem);
            }
            else
            {
                if (item.Id.Equals(userId))
                {
                    this.Remove(item.Id, userId);
                    this.Add(todoItem);
                }
                else
                    throw new TodoAccessDeniedException("User that is trying to access data isn't the owner.");
            }
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            if (_context.TodoItems.Find(todoId).UserId.Equals(userId))
            {
                _context.TodoItems.Find(todoId).MarkAsCompleted();
                _context.SaveChanges();
                return true;
            }
            else
                throw new TodoAccessDeniedException("User that is trying to access data isn't the owner.");
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId.Equals(userId))
                .OrderByDescending(d => d.DateCreated)
                .ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(i => (i.UserId.Equals(userId) && !i.IsCompleted))
                .ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(i => (i.UserId.Equals(userId) && i.IsCompleted))
                .ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(i => (i.UserId.Equals(userId) && filterFunction(i) == true))
                .ToList();
        }
    }
}
