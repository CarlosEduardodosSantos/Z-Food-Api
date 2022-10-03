using System.Threading;
using System.Threading.Tasks;

namespace APIAlturas.BackgroundServices
{
    public class VerificarStatusParceiroServices : BackgroundService
    {
        private readonly RestauranteDAO _restauranteDAO;

        public VerificarStatusParceiroServices(RestauranteDAO restauranteDao)
        {
            _restauranteDAO = restauranteDao;
        }

        protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Do your preparation (e.g. Start code) here
            while (!stoppingToken.IsCancellationRequested)
            {
                _restauranteDAO.AdministracaoDePoolin();
                await Task.Delay(30 * 1000);
                
            }
            //Do your cleanup (e.g. Stop code) here
        }
    }
}