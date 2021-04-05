using ProducerConsumer.DAL;

namespace DAL.Repository
{
    public abstract class BaseRepository
    {
        protected DataBaseContext _context;

        protected BaseRepository()
        {
            _context = new DataBaseContext();
        }
    }
}
