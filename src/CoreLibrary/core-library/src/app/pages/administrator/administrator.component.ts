import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BtspModule } from '../../shared/btsp/btsp.module';
import { CommonModule } from '@angular/common';
import { ITableTemplate, TableTemplate } from '@app/core/models/TableTemplate';
import { UtilsService } from '@app/shared/services/utils.service';
import { DataSource } from '@app/core/Data/data-source';


@Component({
  selector: 'app-administrator',
  standalone: true,
  imports: [BtspModule, CommonModule],
  templateUrl: './administrator.component.html',
  styleUrl: './administrator.component.scss',
})
export class AdministratorComponent implements OnInit {
  constructor(private utils: UtilsService) { }
  private dataSource: DataSource = new DataSource();

  images: string[] = [];
  //Array.from(document.getElementsByTagName('img')).map(a=>a.currentSrc);

  tableTemplate: ITableTemplate | undefined;

  ngOnInit(): void {
    this.images = this.dataSource.getImages();
    this.tableTemplate = {
      dataSource: this.images.map((val: string, index: number) => {
        return {
          id: index,
          data: val,
          name: this.utils.getFileNameFromUrl(val),
        };
      }),
    };

   
  }
}
