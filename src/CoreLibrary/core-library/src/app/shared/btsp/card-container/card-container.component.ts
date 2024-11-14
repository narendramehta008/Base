import {
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { UtilsService } from '@app/shared/services/utils.service';
import { CardComponent, ICardTemplate } from '../card/card.component';
import { FileSaverService } from 'ngx-filesaver';

@Component({
  selector: 'btsp-card-container',
  templateUrl: './card-container.component.html',
  styleUrl: './card-container.component.scss',
})
export class CardContainerComponent implements OnInit, OnChanges {
  @Input() dataSource: CardComponent[] = [];
  @Input() showDownloadAll = false;
  @Input() showExportUrls = false;
  @Input() cardWrapperClass = 'col-sm-6 col-md-6 col-lg-3';

  countedArr = [];

  constructor(
    private utilService: UtilsService,
    private cdr: ChangeDetectorRef,
    private fileSaver: FileSaverService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    for (const inputName in changes) {
      const inputValues = changes[inputName];
    }
  }

  ngOnInit(): void {}

  ngAfterViewChecked(): void {
    this.cdr.detectChanges();
  }

  downloadImages() {
    Array.from(this.dataSource).forEach((item: CardComponent) => {
      item.cards.forEach(async (card: ICardTemplate) => {
        if (card?.media?.src)
          await this.utilService
            .downloadWithFileName(card.media.src)
            .subscribe((res) => {});
      });
    });
  }

  exportUrls() {
    let links: any[] = [];
    this.dataSource.forEach((item, index, arr) => {
      let temp = `${item.cards.map((a) => `"${a?.media?.src}"`).join(',')}`;
      links.push(temp);
    });
    const blob = new Blob([links.join('\n')], { type: 'text/csv' });
    this.fileSaver.save(blob, 'Export Url.csv');
    // const url = window.URL.createObjectURL(blob);
    // window.open(url);
  }
}
