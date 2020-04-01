import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime } from 'rxjs/operators';

import { SelectProduct } from '../../shared/models/select-product.model';

@Component({
  selector: 'mshk-exhibition',
  templateUrl: './exhibition.component.html',
  styleUrls: ['./exhibition.component.scss']
})
export class ExhibitionComponent implements OnInit {
  exhibitionForm: FormGroup;
  exhibitionId: string;
  isEdit = false;
  isLoading = false;
  loadingIndicator = false;
  productsList: SelectProduct[];
  profit: number;

  constructor() {
  }

  ngOnInit() {
    this.readRouteParams();
  }

  save() {
  }

  readRouteParams() {
  }
}
