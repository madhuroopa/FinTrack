import React, { useEffect, useState } from 'react'
import { CompanyCompData } from '../../company';
import { getCompData } from '../../api';
import CompFinderItem from '../CompFinderItem/CompFinderItem';

type Props = {

    ticker:string;

}

const CompFinder = ({ticker}: Props) => {
    const [companyData, setCompanyData]= useState<string[]>();
    useEffect(()=> {
        const getComps= async()=>{
            const value = await getCompData(ticker);
            setCompanyData(value?.data);
        }
        getComps();
    },[ticker]);
    console.log("compfinder data:",companyData)
  return (
    <div className='inline-flex rounded-md shadow-sm m-4'>
        {companyData?.map((ticker:any)=>{
            return <CompFinderItem ticker={ticker}/>
        })}
    </div>
  )
}

export default CompFinder