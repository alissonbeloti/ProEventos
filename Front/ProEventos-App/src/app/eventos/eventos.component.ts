import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  public eventos: any = []
  public eventosFiltrados: any = []
  public wdImg = 150;
  public mgImg = 2;
  mostrarImg = false
  private _filtroLista: string = ''

  public get filtroLista() {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  public getEventos(): void {

    this.http.get(`https://localhost:5001/api/evento`)
      .subscribe(response => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos
      },
        error => console.log(error));
  }

  public mostrarOcultarImagem() {
    this.mostrarImg = !this.mostrarImg
  }
}
