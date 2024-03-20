using RotasService.Entities;


namespace RotasService.Interfaces
{
    public interface IRotaService
    {
        RotaResultadoEntity EncontrarMelhorRota(List<RotaEntity> rotas, string origem, string destino);
        List<RotaEntity> ObterRotas();
    }
}
