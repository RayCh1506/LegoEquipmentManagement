"use client"

import { Fragment, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import EquipmentCard from './EquipmentCard';
import { fetchAllEquipment } from '@/api/api';
import { Equipment } from '@/types/types';
import { useUser } from '@/providers/UserContextProvider';

export default function EquipmentDashboard() {
    const [nameFilter, setNameFilter] = useState('');
    const [locationFilter, setLocationFilter] = useState('');
    const [isOperationalFilter, setIsOperationalFilter] = useState<boolean | undefined>(undefined)
    const { user } = useUser(); 
    
    const { data, error, isLoading } = useQuery<Equipment[]>({
      queryKey: ['equipment-all'],
      queryFn: fetchAllEquipment,
      refetchInterval: 5000, // Poll every 5 seconds for updates, simulates live updates. SignalR is a better alternative
    });

    if (isLoading) return <p className="text-center text-2xl">Loading...</p>;
    if (error) return <p className="text-center text-2xl">Error loading data from the equipment management system</p>;

    if(!data || data.length <= 0){
      return (
        <div className="text-center text-2xl">
            No equipment is available to be displayed
        </div>
      )
    }

    if(!user){
      return (
          <div className="text-center text-2xl">
              You are not logged in, please log in to see access the page
          </div>
      )
    }

    console.log(data);

    const filteredData = data!.filter(equipment => 
        equipment.generalInformation.name.toLowerCase().includes(nameFilter.toLowerCase()) &&
        equipment.generalInformation.location.toLowerCase().includes(locationFilter.toLowerCase()) &&
        (isOperationalFilter === undefined || equipment.operationalInformation.isOperational === isOperationalFilter)
      );

    return (
      <div className="w-screen flex justify-center">
        <div className="max-w-11/12 w-full max-h-[77vh] overflow-y-auto mt-10 border-4 bg-gray-600 rounded-lg p-4">
          <h1 className="text-xl font-bold mb-4 text-center">Equipment list</h1>  
          <div className="mb-4 flex gap-4">
            <input 
            type="text" 
            placeholder="Filter by name" 
            value={nameFilter} 
            onChange={(e) => setNameFilter(e.target.value)} 
            className="border p-2 rounded w-1/3 ml-1 mr-1 bg-white"
            />
            <input 
            type="text" 
            placeholder="Filter by location" 
            value={locationFilter} 
            onChange={(e) => setLocationFilter(e.target.value)} 
            className="border p-2 rounded w-1/3 ml-1 mr-1 bg-white"
            />
            <select 
              value={isOperationalFilter === undefined ? "" : isOperationalFilter.toString()} 
              onChange={(e) => {
                  const value = e.target.value;
                  setIsOperationalFilter(value === "" ? undefined : value === "true");
              }}
              className="border p-2 rounded w-1/3 ml-1 mr-1 bg-white"
            >
              <option value="">All</option>
              <option value="true">Operational</option>
              <option value="false">Not Operational</option>
            </select>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {filteredData!.map((equipment: Equipment) => (
                <Fragment key={equipment.id}>
                    <EquipmentCard equipment={equipment}/>
                </Fragment>
            ))}
            </div>
        </div>
      </div>
      );
    
}