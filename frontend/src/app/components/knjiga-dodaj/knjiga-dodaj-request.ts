export interface KnjigaDodajRequest{
  naslov:string,
  isbn: string,
  opis: string,
  brojStranica:number | null,
  cijena:number | null,
  datumIzdavanja: Date | null,
  naStanju:number | null,
  slikaPutanja: string,
  izdavacID: string | null,
  zanrID: string | null,
  autorIDs: string[]
}
