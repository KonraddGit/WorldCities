import { Component, OnInit } from '@angular/core';

import { City } from './city';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss'],
})
export class CitiesComponent implements OnInit {
  public cities!: City[];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<City[]>(environment.baseUrl + 'api/Cities').subscribe(
      (result) => {
        this.cities = result;
      },
      (error) => console.error(error)
    );
  }
}
