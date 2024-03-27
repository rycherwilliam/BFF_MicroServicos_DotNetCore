import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuscarMelhorRotaComponent } from './buscar-melhor-rota.component';

describe('BuscarMelhorRotaComponent', () => {
  let component: BuscarMelhorRotaComponent;
  let fixture: ComponentFixture<BuscarMelhorRotaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BuscarMelhorRotaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuscarMelhorRotaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
