import React, { SyntheticEvent, useState } from 'react';

import './App.css';

import CardList from './Components/CardList/CradList'
import Search from './Components/Search/Search';
import { CompanySearch } from './company';
import searchCompanies from './api';

function App() {
  const [search,setSearch]=useState<string>("");
  const [searchResult,setSearchResult]=useState<CompanySearch[]>([]);
  const [serverError,setServerError]=useState<string|null>("");
    const handleChange= (e: React.ChangeEvent<HTMLInputElement>) => {

        setSearch(e.target.value)
        console.log(e)
    }
   const onClick =async (e: SyntheticEvent)=>{
    const result = await searchCompanies(search)
    console.log(result)
    if (typeof(result)==="string"){
      
      setServerError(result)
    }
    else if (Array.isArray(result.data)){
     
      setSearchResult(result.data);
    }
    
    console.log(searchResult)
 
   };
  return (
    
    <div className="App">
      
      <Search onClick={onClick} search={search} handleChange={handleChange}/>
      {serverError && <h1>{serverError}</h1>}
      <CardList searchResults={searchResult}/>

    </div>
  );
}

export default App;
