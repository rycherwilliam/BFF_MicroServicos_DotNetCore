using Newtonsoft.Json;
using RotasService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotasService.UseCases
{
    public static class RotasFile
    {       
        public static List<RotaEntity> LerRotas()
        {
            const string _caminhoArquivo = "utils/rotas.json";
            string json = File.ReadAllText(_caminhoArquivo);
            return JsonConvert.DeserializeObject<List<RotaEntity>>(json);
        }
    }
}
