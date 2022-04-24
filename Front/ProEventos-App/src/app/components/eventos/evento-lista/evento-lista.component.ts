import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  public eventos: Evento[] = []
  public eventosFiltrados: Evento[] = []
  public wdImg = 150;
  public mgImg = 2;
  mostrarImg = false
  private _filtroLista: string = ''
  modalRef?: BsModalRef;
  eventoId: number = 0;

  public get filtroLista() {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos
  }

  constructor(private evento: EventoService,
    private modalService: BsModalService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router) { }

  public ngOnInit(): void {
    this.getEventos();
    /** spinner starts on init */
    this.spinner.show();

    setTimeout(() => {
      /** spinner ends after 5 seconds */
      this.spinner.hide();
    }, 5000);
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  public getEventos(): void {
    this.evento.getEventos()
      .subscribe({
        next: (evt: Evento[]) => {
          this.eventos = evt;
          this.eventosFiltrados = this.eventos;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar os Eventos', 'Error');
        },
        complete: () => this.spinner.hide()
      })
  }

  public mostrarOcultarImagem(): void {
    this.mostrarImg = !this.mostrarImg
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.spinner.show();
    this.evento.deleteEvento(this.eventoId)
      .subscribe((result: any) => {
        if (result.message === 'Deletado.') {
          this.toastr.success('O Evento foi deletado com Sucesso.', `Deletado!`);
          this.getEventos();
        }
      },
        (error: any) => {
          console.error(error)
          this.toastr.error(`Erro ao tentar deletar o evento: ${this.eventoId}`)
        })
      .add(() => this.spinner.hide())
    this.modalRef?.hide();
    this.toastr.success('O evento foi deletado com sucesso', 'Deletado!');
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }
}
