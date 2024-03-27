import { Component, OnInit } from '@angular/core';
import { BuscarMelhorRotaService } from './buscar-melhor-rota.service';
import { RotaEntity, RotaResultadoEntity } from './buscar-melhor-rota.model';

@Component({
  selector: 'app-buscar-melhor-rota',
  templateUrl: './buscar-melhor-rota.component.html',
  styleUrls: ['./buscar-melhor-rota.component.css']
})
export class BuscarMelhorRotaComponent implements OnInit {
  origemSelecionada: RotaEntity | null = null;
  destinoSelecionado: RotaEntity | null = null; 
  melhorRota: RotaResultadoEntity = { rotas: [], custo: 0 };
  rotaNaoEncontrada: boolean = false;
  rotasDisponiveis: RotaEntity[] = [];
  melhorRotaFormatada: string = '';

  constructor(private rotasService: BuscarMelhorRotaService) {}
  
  ngOnInit(): void {
    this.carregarRotasDisponiveis();
  }

  carregarRotasDisponiveis() {
    this.rotasService.obterRotasDisponiveis().subscribe((rotas) => {      
      this.rotasDisponiveis = rotas.map(rota => {
        const entidadeRota: RotaEntity = {
          origem: rota.origem,
          destino: rota.destino,
          valor: rota.valor
        };
        return entidadeRota;
      });
    });
  }

  buscarMelhorRota() {
    const observer = {
      next: (resultado: RotaResultadoEntity) => {
        this.melhorRota = resultado;
        this.formatarMelhorRota();
        this.rotaNaoEncontrada = false;
      },
      error: (error: any) => {
        console.error('Erro ao buscar rota:', error);
        this.rotaNaoEncontrada = true;
      }
    };
  
    if (this.origemSelecionada && this.destinoSelecionado) {      
      this.rotasService.obterMelhorRota(this.origemSelecionada.origem, this.destinoSelecionado.destino)
        .subscribe(observer);
    } else {
      console.error('Origem ou destino não selecionados.');
    }
  }
  
  formatarMelhorRota(): void {
    if (this.melhorRota.rotas.length === 0) {
      this.melhorRotaFormatada = 'Nenhuma rota encontrada.';
      return;
    }
  
    let rotaCompleta = '';
    
    for (let i = 0; i < this.melhorRota.rotas.length; i++) {
      const rota = this.melhorRota.rotas[i];
      rotaCompleta += rota;     
      
      if (i !== this.melhorRota.rotas.length - 1) {
        rotaCompleta += ' - ';
      }
    }   
    
    this.melhorRotaFormatada = rotaCompleta + ' ao custo de $' + this.melhorRota.custo;
  }
}