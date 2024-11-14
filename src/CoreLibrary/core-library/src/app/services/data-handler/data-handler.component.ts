import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { CardComponent } from '@app/shared/btsp/card/card.component';
import { UtilsService } from '@app/shared/services/utils.service';

export interface IKeyValue<TKey, TValue> {
  id: TKey;
  value: TValue;
}
@Component({
  selector: 'app-data-handler',
  templateUrl: './data-handler.component.html',
  styleUrl: './data-handler.component.scss',
})
export class DataHandlerComponent implements OnInit {
  @ViewChild('dataForm', { static: true }) dataForm: NgForm | undefined;
  dataFormGroup: FormGroup = new FormGroup({});
  results?: string[] = [];
  resultsString?: string = '';
  dataTypes: IKeyValue<number, string>[] = [
    {
      id: 1,
      value: 'JSON',
    },
    {
      id: 2,
      value: 'HTML',
    },
  ];
  predefinedOperations: IKeyValue<number, string>[] = [
    {
      id: 1,
      value: 'Fetch Images',
    },
  ];
  errorMessage = '';
  dataSource: CardComponent[] = [];

  constructor(private utils: UtilsService) {}

  ngOnInit(): void {
    this.dataFormGroup.addControl(
      'type',
      new FormControl(1, [Validators.pattern('^[1-2]*$')])
    );
    this.dataFormGroup.addControl('data', new FormControl(''));
    this.dataFormGroup.addControl(
      'predefinedOperations',
      new FormControl(1, [Validators.pattern('^[1]*$')])
    );
  }

  onSubmit() {
    if (this.dataFormGroup.valid) {
      this.resetData();
      if (this.dataFormGroup.value['type'] == 1) this.jsonOperations();
      else this.htmlOperations();
    } else {
      // this.dataFormGroup.errors
      this.errorMessage = '';
    }
  }
  jsonOperations() {}
  htmlOperations() {
    const parser = new DOMParser();
    const htmlDoc = parser.parseFromString(
      this.dataFormGroup.controls['data'].value,
      'text/html'
    );
    this.results = Array.from(htmlDoc.getElementsByTagName('img')).map(
      (a) => a.currentSrc || a.src
    );
    this.resultsString = this.results.join('\n');
    let cards = this.results.map((item) =>
      new CardComponent(this.utils).setCardValue({
        media: { src: item },
      })
    );
    this.dataSource.push(...cards);
  }

  resetData() {
    this.errorMessage = '';
    this.dataSource = [];
    this.results = [];
    this.resultsString = '';
  }
}
