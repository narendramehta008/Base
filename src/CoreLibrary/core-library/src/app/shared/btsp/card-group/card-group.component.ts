import { Component, Input, TemplateRef } from '@angular/core';
import { UtilsService } from '@app/shared/services/utils.service';
import { ICardTemplate, CardComponent } from '../card/card.component';

@Component({
  selector: 'app-card-group',
  templateUrl: './card-group.component.html',
  styleUrl: './card-group.component.scss',
})
export class CardGroupComponent {
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

  setCardValue(card?: ICardTemplate) {
    if (!card) return this;
    this.cards.push(this.setCardDefault(card));
    return this;
  }
  // setCards(cards?: ICardTemplate[]) {
  //   cards?.map((card) => {
  //     this.cards.push(this.setCardDefault(card));
  //   });
  //   return this;
  // }

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
