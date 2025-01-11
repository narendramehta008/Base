import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BtspModule } from '../../shared/btsp/btsp.module';
import { CommonModule } from '@angular/common';
import { UtilsService } from '@app/shared/services/utils.service';
import { DataSource } from '@app/core/Data/data-source';
import { TableTemplate } from '@app/shared/btsp/core/TableTemplate';


@Component({
  selector: 'app-administrator',
  standalone: true,
  imports: [BtspModule, CommonModule],
  templateUrl: './administrator.component.html',
  styleUrl: './administrator.component.scss',
})
export class AdministratorComponent implements OnInit {
  constructor(private utils: UtilsService, private cdr: ChangeDetectorRef) {
    this.tableTemplate = new TableTemplate(cdr);
  }
  private dataSource: DataSource = new DataSource();

  images: string[] = [];
  //Array.from(document.getElementsByTagName('img')).map(a=>a.currentSrc);

  tableTemplate: TableTemplate;

  ngOnInit(): void {
    this.images = this.dataSource.getImages();
    this.tableTemplate.dataSource.set(this.images.map((val: string, index: number) => {
      return {
        id: index,
        data: val,
        name: this.utils.getFileNameFromUrl(val),
      };
    }));


  }
}
