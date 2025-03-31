import EquipmentDashboard from "@/components/EquipmentDashboard";
import { dehydrate, HydrationBoundary } from "@tanstack/react-query";
import { getQueryClient } from "../utils/getQueryClient";
import { fetchAllEquipment } from "@/api/api";

export default async function Dashboard() {
  
  const queryClient = getQueryClient();

  await queryClient.prefetchQuery({
    queryKey: ["equipment-all"],
    queryFn: async () => {
      const equipmentList = await fetchAllEquipment();
      equipmentList.forEach((equipment) => {
        queryClient.setQueryData([`equipment-${equipment.id}`], equipment)
      })
      return equipmentList;
    }
  });

  return (
    <>
      <h1 className="text-4xl md:text-5xl font-bold mb-5 text-center">DASHBOARD</h1>
      <HydrationBoundary state={(dehydrate(queryClient))}>
        <EquipmentDashboard/>
      </HydrationBoundary>
    </>

  );
}

