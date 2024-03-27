import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RotaEntity, RotaResultadoEntity } from './buscar-melhor-rota.model';

@Injectable({
  providedIn: 'root'
})
export class BuscarMelhorRotaService {

  private apiUrl = 'https://localhost:7214/api/rotas';

  constructor(private http: HttpClient) { }

  obterMelhorRota(origem: string, destino: string): Observable<RotaResultadoEntity> {
    return this.http.get<RotaResultadoEntity>(`${this.apiUrl}/${origem}/${destino}`);
  }

  obterRotasDisponiveis(): Observable<RotaEntity[]> {
    return this.http.get<RotaEntity[]>(`${this.apiUrl}/rotas-disponiveis`);
  }
}
