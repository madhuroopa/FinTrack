import React from 'react'
import Table from '../../Components/Table/Table'
import RatioList from '../../Components/RatioList/RatioList'
import { CompanyKeyMetrics } from '../../company';
import { testIncomeStatementData } from '../../Components/Table/testData';

type Props = {}
const tableConfig = [
  {
    label: "Market Cap",
    render: (company: CompanyKeyMetrics) => company.marketCapTTM,
    subTitle: "Total value of all a company's shares of stock",
  }
];

const DesignPage = (props: Props) => {
  return (
    <>
    <h1>FinTrack Design Page</h1>
    <h2> THis is FinTrack's design Page. This is where we well house various deign aspects of the app </h2>
    
    <RatioList data ={testIncomeStatementData} config ={tableConfig}/>
        <Table data ={testIncomeStatementData} config ={tableConfig}/>
    </>
  )
}

export default DesignPage