import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { IKeyValue } from '@app/core/models/Funcs';
import { ITableTemplate, TableTemplate } from '@app/core/models/TableTemplate';
import {
  CardComponent,
} from '@app/shared/btsp/card/card.component';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment.development';
import { search } from '@metrichor/jmespath';


@Component({
  selector: 'app-data-handler',
  templateUrl: './data-handler.component.html',
  styleUrl: './data-handler.component.scss',
})
export class DataHandlerComponent implements OnInit {
  @ViewChild('dataForm', { static: true }) dataForm: NgForm | undefined;
  dataFormGroup: FormGroup = new FormGroup({});
  results: string[] = [];
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
    {
      id: 2,
      value: 'Query',
    },
  ];
  errorMessage = '';
  dataSource: CardComponent[] = [];
  tableTemplate: ITableTemplate = { dataSource: [] };

  constructor(private utils: UtilsService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.dataFormGroup.addControl(
      'type',
      new FormControl(1, [Validators.pattern('^[1-2]*$')])
    );
    this.dataFormGroup.addControl('data', new FormControl(''));
    this.dataFormGroup.addControl(
      'query',
      new FormControl(
        `results[*].{media:{src:image_url,type:'Image'},title:title,text:seo_title_formatted, onHoverShowDetails:'true', showDownload:'true'}`
      )
    );
    this.dataFormGroup.addControl('apiUrl', new FormControl());
    this.dataFormGroup.addControl('showCards', new FormControl(true));
    this.dataFormGroup.addControl('populateTable', new FormControl(false));
    this.dataFormGroup.addControl(
      'predefinedOperations',
      new FormControl(1, [Validators.pattern('^[1-2]*$')])
    );
  }

  executeApi() {
    const url = this.getControlValue('apiUrl');
    if (url) {
      this.utils
        .postRequest(environment.apiEndPoint.apiManager.get, {
          url: url,
          headers: [
            {
              key: 'User-Agent',
              value:
                'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36',
            },
          ],
        })
        .subscribe((res) => {
          let result: string = '';
          console.log(res);
          if (typeof res == 'string') result = res;
          else result = JSON.stringify(res);
          this.getControl('data').setValue(result);
        });
    }
  }

  executeQuery() {
    if (this.getControlValue('type') == 1) this.jsonOperations();
    else this.htmlOperations();
  }

  onSubmit() {
    if (this.dataFormGroup.valid) {
      this.resetData();
      if (this.getControlValue('type') == 1) this.jsonOperations();
      else this.htmlOperations();
      this.getControlValue('showCards') && this.populateCards();
      this.getControlValue('populateTable') && this.populateTable();
    } else {
      // this.dataFormGroup.errors
      this.errorMessage = '';
    }
  }
  jsonOperations() {
    this.results = [];
    if (!this.getControlValue('data')) return;
    const results: any = search(
      JSON.parse(this.getControlValue('data')),
      this.getControlValue('query')
    );
    if (results) {
      this.results.push(
        ...Array.from(results).map((a) =>
          typeof a == 'string' ? a : JSON.stringify(a)
        )
      );
      this.resultsString = this.results.join('\n');
    }
  }

  htmlOperations() {
    const data = this.getControlValue('data');
    if (!data) return;

    if(this.getControlValue('predefinedOperations')==1){
      const parser = new DOMParser();
      const htmlDoc = parser.parseFromString(data,
        'text/html'
      );
  
      this.results = Array.from(htmlDoc.getElementsByTagName('img')).map(
        (a) => a.currentSrc || a.src
      );
    }
    else{
      const parser = new DOMParser();
      const htmlDoc = parser.parseFromString(data,
        'text/html'
      );
      let results = htmlDoc.querySelector(this.getControlValue('query'));
      console.log(results);
      if (results) {
        this.results.push(
          ...Array.from(results).map((a) =>
            typeof a == 'string' ? a : JSON.stringify(a)
          )
        );
        this.resultsString = this.results.join('\n');
      }
    }

    
   
    
  }

  populateCards() {
    this.resultsString = this.results.join('\n');
    let cards = this.results.map((item) => {
      if (item.match('{')) {
        const parsed = JSON.parse(item);
        return new CardComponent(this.utils).setCardValue(parsed);
      } else
        return new CardComponent(this.utils).setCardValue({
          media: { src: item, type: 'Image' },
        });
    });
    this.dataSource.push(...cards);
  }

  populateTable() {
    this.resultsString = this.results.join('\n');
    this.tableTemplate.dataSource = this.results.map((item) =>
      JSON.parse(item)
    );
    this.tableTemplate = new TableTemplate(this.tableTemplate, this.cdr);
  }

  resetData() {
    this.errorMessage = '';
    this.dataSource = [];
    this.results = [];
    this.resultsString = '';
  }
  getControlValue(controlName: string) {
    return this.getControl(controlName).value;
  }
  getControl(controlName: string) {
    return this.dataFormGroup.controls[controlName];
  }
}
