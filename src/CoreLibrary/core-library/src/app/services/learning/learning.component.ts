import { Component, OnInit } from '@angular/core';
import { DataSource } from '@app/core/Data/data-source';
import { Summary } from '@app/core/models/Funcs';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-learning',
  templateUrl: './learning.component.html',
  styleUrl: './learning.component.scss',
})
export class LearningComponent implements OnInit {

  constructor(private utils: UtilsService) {

  }
  expanded = false;
  summarys: Summary[] = [];

  ngOnInit(): void {

    this.utils.getRequest(environment.apiEndPoint.data.get, {
      type: 'Summary'
    }).subscribe({
      next: (response: Summary[]) => {
        this.summarys = this.utils.makeSummaryTree(response, 1);
        console.log(this.summarys);
      }, error: (error) => {
        console.log(error);
      }
    });

    this.summarys = new DataSource().getSummaries();

  }

}
