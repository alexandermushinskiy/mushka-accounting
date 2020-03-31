import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ExpenseCategory } from '../../../../shared/enums/expense-category.enum';

@Component({
  selector: 'mshk-select-category',
  templateUrl: './select-category.component.html',
  styleUrls: ['./select-category.component.scss']
})
export class SelectCategoryComponent implements OnInit {
  @Input() disabled = false;
  @Input() placeholder = 'common.selectCategory';
  @Output() onCategorySelected = new EventEmitter<ExpenseCategory>();
  selectedCategory: ExpenseCategory;

  categories = [
    { id: ExpenseCategory.ADVERTISING, name: 'expenceCategory.advertising' },
    { id: ExpenseCategory.DESIGN, name: 'expenceCategory.design' },
    { id: ExpenseCategory.EQUIPMENT, name: 'expenceCategory.equipment' },
    { id: ExpenseCategory.PHOTOGRAPHY, name: 'expenceCategory.photography' },
    { id: ExpenseCategory.POLYGRAPHY, name: 'expenceCategory.polygraphy' },
    { id: ExpenseCategory.PROMO, name: 'expenceCategory.promo' },
    { id: ExpenseCategory.WEBSITE, name: 'expenceCategory.website' },
    { id: ExpenseCategory.OTHER, name: 'expenceCategory.other' }
  ];

  constructor() {
  }

  ngOnInit() {
  }

  onCategoryChanged(category: ExpenseCategory) {
    this.selectedCategory = category;
    this.emitCategory();
  }

  clear() {
    this.selectedCategory = null;
    this.emitCategory();
  }

  private emitCategory() {
    this.onCategorySelected.emit(this.selectedCategory);
  }
}
