import React, { useEffect, useState } from 'react'
import { CompanyTenK } from '../../company';
import { getTenK } from '../../api';
import TenKFinderItem from '../TenKFinderItem/TenKFinderItem';
import Spinner from '../Spinner/Spinner';

type Props = {
    ticker:string;
}

const TenkFinder = ({ticker}: Props) => {
    const [companyData,setCompanyData]=useState<CompanyTenK[]>();
    useEffect(()=>{
        const getTenkData = async ()=>{
            const value = await getTenK(ticker);
            setCompanyData(value?.data)
        };
        getTenkData();
    },[ticker]);
  return (
    <div className="inline-flex rounded-md shadow-sm m-4" role="group">
    {companyData ? (
      companyData?.slice(0, 5).map((tenK) => {
        return <TenKFinderItem tenK={tenK} />;
      })
    ) : (
      <Spinner />
    )}
  </div>
);
}

export default TenkFinder