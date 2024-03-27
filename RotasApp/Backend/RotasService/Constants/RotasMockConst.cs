using RotasService.Entities;

namespace RotasService.Constants
{
    public static class RotasMockConst
    {
        public static readonly List<RotaEntity> RotasMock = new List<RotaEntity>
        {
            new RotaEntity { Origem = "GRU", Destino = "BRC", Valor = 10 },
            new RotaEntity { Origem = "BRC", Destino = "SCL", Valor = 5 },
            new RotaEntity { Origem = "GRU", Destino = "CDG", Valor = 75 },
            new RotaEntity { Origem = "GRU", Destino = "SCL", Valor = 20 },
            new RotaEntity { Origem = "GRU", Destino = "ORL", Valor = 56 },
            new RotaEntity { Origem = "ORL", Destino = "CDG", Valor = 5 },
            new RotaEntity { Origem = "SCL", Destino = "ORL", Valor = 20 }
        };
    }
}
