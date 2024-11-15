import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { UtilsService } from '@app/shared/services/utils.service';

export interface IMedia {
  src?: string;
  type?: 'Audio' | 'Video' | 'Image';
  ext?: string;
}

export interface ICardTemplate {
  media?: IMedia;
  title?: string;
  text?: string;
  redirectUrl?: string;
  redirectName?: string;
  showDownload?: boolean;
  onHoverShowDetails?: boolean;
  fontClass?: string;
  cardClass?: string;
}

@Component({
  selector: 'btsp-card',
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss',
})
export class CardComponent implements OnInit {
  @Input() cards: ICardTemplate[] = [];
  @Input() cardTemplate: CardComponent | undefined;

  customCardTemplate: TemplateRef<any> | undefined;
  currentIndex = 0;

  constructor(private utilService: UtilsService) {}

  ngOnInit(): void {
    if (this.cardTemplate) {
      // this.cardTemplate.cards = this.cardTemplate.cards;
      this.customCardTemplate = this.cardTemplate.customCardTemplate;
    }
  }

  setCardValue(card?: ICardTemplate, defaultValue?: boolean) {
    if (!card) return this;
    if (defaultValue) this.cards.push(this.setCardDefault(card));
    else {
      card.media &&
        (card.media = {
          src: card.media.src,
          type: card.media.type || 'Image',
        });
      this.cards.push(card);
    }
    return this;
  }

  setCardDefault(card: ICardTemplate) {
    card.title = card.title || 'title';
    card.text =
      card.text ||
      "Some quick example text to build on the card title and make up the bulk of the card's content.";
    card.redirectName = card.redirectName || 'Link';
    card.showDownload = false;
    card.media &&
      (card.media = { src: card.media.src, type: card.media.type || 'Image' });
    return card;
  }

  downloadMedia(mediaSrc?: string) {
    if (!mediaSrc) {
      //alert invalid media src
      return;
    }
    // window.open(this.imageSrc);
    this.utilService.downloadWithFileName(mediaSrc).subscribe((res) => {});
    // this.utilService.downloadWithResponseFileName(this.imageSrc);
  }
}
