"use strict";
//@ts-check

import { data } from "./main.js";

function ListazClick() {
    // Get the div where you want to append the table
    const divElement = document.getElementById('table');
 
    const OsszesPontszamInput = document.getElementById('OsszesPontszamInput').value;
    
   
    // Check if OsszesPontszamInput is a valid number
    if (OsszesPontszamInput !== '' && !isNaN(OsszesPontszamInput)) {
        const existingTable = document.getElementById('dynamic-table');
        if(OsszesPontszamInput > 100){
            return;
        }
        if (existingTable) {
          divElement.removeChild(existingTable);
        }

    // Create a table element
    const table = document.createElement('table');
    table.id = 'dynamic-table';

    // Create a header row
    const headerRow = document.createElement('tr');

    const OM_Azonosito = document.createElement('th');
    OM_Azonosito.textContent = 'OM Azonosító';
    headerRow.appendChild(OM_Azonosito);

    const Neve = document.createElement('th');
    Neve.textContent = 'Neve';
    headerRow.appendChild(Neve);

    const ErtesitesiCime = document.createElement('th');
    ErtesitesiCime.textContent = 'Értesitesi Címe';
    headerRow.appendChild(ErtesitesiCime);

    const Email = document.createElement('th');
    Email.textContent = 'Email';
    headerRow.appendChild(Email);

    const SzuletesiDatum = document.createElement('th');
    SzuletesiDatum.textContent = 'Születési Dátum';
    headerRow.appendChild(SzuletesiDatum);

    const Matematika = document.createElement('th');
    Matematika.textContent = 'Matematika';
    headerRow.appendChild(Matematika);

    const Magyar = document.createElement('th');
    Magyar.textContent = 'Magyar';
    headerRow.appendChild(Magyar);

    const OsszesPontszam = document.createElement('th');
    OsszesPontszam.textContent = 'Összes Pontszám';
    headerRow.appendChild(OsszesPontszam)

    table.appendChild(headerRow);

    // Create a body for the table
    const tableBody = document.createElement('tbody');

    // Add data rows
    const sortedData = data.filter(dataItem => dataItem.Matematika + dataItem.Magyar >= OsszesPontszamInput).sort((a, b) => a.Neve.localeCompare(b.Neve));

    sortedData.forEach(dataItem => {
      const dataRow = document.createElement('tr');

      const OM_AzonositoCell = document.createElement('td');
      OM_AzonositoCell.textContent = dataItem.OM_Azonosito;
      dataRow.appendChild(OM_AzonositoCell);

      const NeveCell = document.createElement('td');
      NeveCell.textContent = dataItem.Neve;
      dataRow.appendChild(NeveCell);

            const ErtesitesiCimeCell = document.createElement('td');
            ErtesitesiCimeCell.textContent = dataItem.ErtesitesiCime;
            dataRow.appendChild(ErtesitesiCimeCell);

            const EmailCell = document.createElement('td');
            EmailCell.textContent = dataItem.Email;
            dataRow.appendChild(EmailCell);

            const SzuletesiDatumCell = document.createElement('td');
            SzuletesiDatumCell.textContent = dataItem.SzuletesiDatum;
            dataRow.appendChild(SzuletesiDatumCell);

            const MatematikaCell = document.createElement('td');
            MatematikaCell.textContent = dataItem.Matematika;
            dataRow.appendChild(MatematikaCell);

            const MagyarCell = document.createElement('td');
            MagyarCell.textContent = dataItem.Magyar;
            dataRow.appendChild(MagyarCell);

            const OsszesPontszamCell = document.createElement('td');
            OsszesPontszamCell.textContent = dataItem.Matematika + dataItem.Magyar;
            dataRow.appendChild(OsszesPontszamCell);

            tableBody.appendChild(dataRow);
        });

    
    table.appendChild(tableBody);

    // Append the table to the div
    divElement.appendChild(table);

    
}}

// Máskülönben nem találja a html
window.ListazClick = ListazClick;