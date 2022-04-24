import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;
  form: FormGroup;
  estadoSalvar: string = 'post';
  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true, adaptivePosition: true, dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) {

    this.form = new FormGroup({});
    this.validation();
    this.localeService.use('pt-br');
  }
  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if (eventoIdParam !== null) {
      this.spinner.show();

      this.estadoSalvar = 'put';
      this.eventoService.getEventosById(+eventoIdParam)
        .subscribe({
          next: (evento: Evento) => {
            this.evento = { ...evento }; //gera nova instancia ...
            this.form.patchValue(this.evento);
          },
          error: (error: any) => {
            console.error(error);
            this.toastr.error('erro ao tentar carregar evento')
            this.spinner.hide()
          },
          complete: () => { this.spinner.hide() },
        });
    }
  }
  ngOnInit(): void {
    this.carregarEvento();

  }

  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemUrl: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }
  /**
   * Salvar
   */
  public salvarAlteracao(): void {
    this.spinner.show();
    var metodo: Observable<Evento>;
    if (this.form.valid) {

      if (this.estadoSalvar == 'post') {
        this.evento = { ...this.form.value }
        metodo = this.eventoService.post(this.evento);
      }
      else {
        this.evento = { id: this.evento.id, ...this.form.value }
        metodo = this.eventoService.put(this.evento);
      }

      metodo.subscribe(
        () => { this.toastr.success('Evento salvo com sucesso!', 'Sucesso') },
        (error: any) => {
          console.error(error)
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento.', 'Erro')
        },
        () => this.spinner.hide(),
      )
    }
  }

}
