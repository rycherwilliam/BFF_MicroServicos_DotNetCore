using RotasService.Entities;
using RotasService.Interfaces;

namespace RotasService.UseCases
{
    public class RotasMelhorCusto : IRotaService
    {
        public RotaResultadoEntity EncontrarMelhorRota(List<RotaEntity> rotas, string origem, string destino)
        {            

            var todasRotas = new List<RotaResultadoEntity>();
            EncontrarRotas(rotas, origem, destino, new List<string>(), todasRotas);

            // no final ordena todas as rotas encontradas por custo e seleciona a mais barata
            var melhorRota = todasRotas.OrderBy(r => r.Custo).FirstOrDefault();
            return melhorRota;
        }

        private void EncontrarRotas(List<RotaEntity> rotas, string origem, string destino, List<string> rotaAtual, List<RotaResultadoEntity> todasRotas)
        {
            // adiciona a origem à rota atual
            rotaAtual.Add(origem);

            // verifica se a origem é igual ao destino
            if (origem == destino)
            {
                var custoTotal = CalcularCustoTotal(rotas, rotaAtual);
                todasRotas.Add(new RotaResultadoEntity { Rotas = rotaAtual.ToArray(), Custo = custoTotal });
                return;
            }

            //encontra todas as rotas para o destino
            foreach (var rota in rotas.Where(r => r.Origem == origem))
            {
                // verifica se a rota atual é o destino
                if (!rotaAtual.Contains(rota.Destino))
                {
                    var novaRotaAtual = new List<string>(rotaAtual);
                    EncontrarRotas(rotas, rota.Destino, destino, novaRotaAtual, todasRotas);
                }
            }
        }

        private decimal CalcularCustoTotal(List<RotaEntity> rotas, List<string> rota)
        {
            decimal custoTotal = 0;
            for (int i = 0; i < rota.Count - 1; i++)
            {
                var rotaAtual = rotas.FirstOrDefault(r => r.Origem == rota[i] && r.Destino == rota[i + 1]);
                if (rotaAtual != null)
                {
                    custoTotal += rotaAtual.Valor;
                }
                else
                {
                    // se há rota não existir retornar um valor grande
                    return decimal.MaxValue;
                }
            }
            return custoTotal;
        }

        public List<RotaEntity> ObterRotas()
        {            
            return RotasFile.LerRotas();
        }
    }    
}

