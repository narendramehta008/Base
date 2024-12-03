import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Summary } from '@app/core/models/Funcs';
import { ITableTemplate, TableTemplate } from '@app/core/models/TableTemplate';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-json-handler',
  templateUrl: './json-handler.component.html',
  styleUrl: './json-handler.component.scss',
})
export class JsonHandlerComponent implements AfterViewInit {
  parents: Summary[] = [];
  tableTemplate: ITableTemplate = { dataSource: [] };

  constructor(private utils: UtilsService, private cdr: ChangeDetectorRef) {}
  ngAfterViewInit(): void {
    this.loadData();
  }

  //https://localhost:7050/api/Summary/GetParents
  //https://localhost:7050/api/Summary/GetParents
  loadData() {
    this.utils
      .getRequest(environment.apiEndPoint.summary.getParents)
      .subscribe({
        next: (response: Summary[]) => {
          this.parents = response;
        },
        error: (error: HttpErrorResponse) => {
          this.parents = [];
          console.log(error);
        },
      });
  }

  populateChilds(id: number) {
    console.log('load data');
    this.utils
      .getRequest(
        this.utils.format(environment.apiEndPoint.summary.getChilds, id)
      )
      .subscribe({
        next: (response: Summary[]) => {
          this.tableTemplate.dataSource = response[0].summaries;
          this.tableTemplate = new TableTemplate(this.tableTemplate, this.cdr);
        },
        error: (error: HttpErrorResponse) => {
          this.parents = [];
          console.log(error);
        },
      });
  }
}
