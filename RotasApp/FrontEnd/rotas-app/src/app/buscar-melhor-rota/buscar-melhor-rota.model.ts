export interface RotaResultadoEntity {
    rotas: RotaEntity[];
    custo: number;
}

export interface RotaEntity{
    origem: string; 
    destino: string;
    valor: number; 
}