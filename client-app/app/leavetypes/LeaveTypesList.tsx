async function getData() {
    const res = await fetch('http://localhost:5000/LeaveTypes')
    // The return value is *not* serialized
    // You can return Date, Map, Set, etc.
   
    if (!res.ok) {
      // This will activate the closest `error.js` Error Boundary
      throw new Error('Failed to fetch data')
    }
   
    return res.json()
  }
   
  export default async function LeaveTypesList() {
    const datas = await getData()
   
    return(
        <>
        {datas.map((data : any)=>(
            <div key={data.id} className="card my-5">
                <h3>{data.name}</h3>
                <h3>{data.dateCreated}</h3>
                <h3>{data.dateModified}</h3>
                <h3>{data.defaultDays}</h3>
            </div>
        ))}
        </>
    )
        
  }