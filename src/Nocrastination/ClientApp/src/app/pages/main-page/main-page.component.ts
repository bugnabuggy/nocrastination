import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html'
})
export class MainPageComponent implements OnInit {
  constructor(
    private titleSvc: Title
  ){}

  ngOnInit() {
    this.titleSvc.setTitle('Nocrastinaton');
  }
} 
