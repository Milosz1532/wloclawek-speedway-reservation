using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseAccess.Models;
using System.Runtime.ConstrainedExecution;

namespace DataBase
{
	public static class Access
	{
		private static string LoadConnectionString()
		{
			return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

			//if (Debugger.IsAttached)
			//{
			//	return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
			//}
			//else
			//{
			//	return ConfigurationManager.ConnectionStrings["AppData"].ConnectionString;
			//}
		}

		public static List<CarModel> LoadCars()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<CarModel>("select * from car", new DynamicParameters());

				return output.ToList();
			}
		}

		public static List<OfferModel> LoadReservationOffer()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<OfferModel>("select * from offer", new DynamicParameters());

				return output.ToList();
			}
		}

		public static List<WorkdayModel> LoadWorkdays(DateTime DateStart, DateTime DateEnd)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var parameters = new { DateStart = DateStart, DateEnd = DateEnd };

				var output = cnn.Query<WorkdayModel>("select * from workday where date(date) >= date(@DateStart) and date(date) <= date(@DateEnd)", parameters);

				return output.ToList();
			}
		}

		public static void AddWorkday(WorkdayModel workday)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				cnn.Execute("insert into workday(date,hour_start,hour_end) values(@date,@hour_start,@hour_end)", workday);
			}
		}

		public static void DeleteWorkday(WorkdayModel workday)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				cnn.Execute("delete from workday where id=@id", workday);
			}
		}

		public static void UpdateWorkday(WorkdayModel workday)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				cnn.Execute("update workday set hour_start=@hour_start, hour_end=@hour_end where id=@id", workday);
			}
		}

		public static List<ReservationViewModel> LoadReservationsView(DateTime DateStart, DateTime DateEnd)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var parameters = new { DateStart = DateStart, DateEnd = DateEnd };

				var output = cnn.Query<ReservationViewModel>("select * from v_reservations_summary where date(date) >= date(@DateStart) and date(date) <= date(@DateEnd)", parameters);

				return output.ToList();
			}
		}

		public static void AddReservation(ReservationModel reservation, ClientModel client, List<CarModel> carsList)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var addClient = cnn.Execute("REPLACE INTO client(pesel,first_name,last_name)  VALUES(@pesel,@first_name,@last_name)", client);
				int addReservation = cnn.ExecuteScalar<int>("insert into reservation(client_pesel,date,hour_start,hour_end,people_count,offer_id) values(@client_pesel,@date,@hour_start,@hour_end,@people_count,@offer_id);select last_insert_rowid();", reservation);
				if (carsList.Count > 0)
				{
					foreach (CarModel car in carsList)
					{
						var parameters = new { car_id = car.id, reservation_id = addReservation };
						var addVehicles = cnn.Execute("insert into car__reservation(car_id,reservation_id) values (@car_id,@reservation_id)", parameters);
					}
				}
			}
		}

        public static void EditReservation(ReservationModel reservation, ClientModel client, List<CarModel> carsList)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var addClient = cnn.Execute("REPLACE INTO client(pesel,first_name,last_name)  VALUES(@pesel,@first_name,@last_name)", client);
                int addReservation = cnn.ExecuteScalar<int>("UPDATE reservation SET client_pesel=@client_pesel, date=@date,hour_start=@hour_start,hour_end=@hour_end,people_count=@people_count,offer_id=@offer_id WHERE id=@id", reservation);
                var rId = new { reservation_id = reservation.id };
                cnn.Execute("DELETE FROM car__reservation WHERE reservation_id=@reservation_id", rId);
                if (carsList.Count > 0)
                {
                    foreach (CarModel car in carsList)
                    {
                        var parameters = new { car_id = car.id, reservation_id = reservation.id };
                        var addVehicles = cnn.Execute("insert into car__reservation(car_id,reservation_id) values (@car_id,@reservation_id)", parameters);
                    }
                }
            }
        }

        public static void AddVehicle(CarModel vehicle)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Execute("insert into car(model,production_year,engine_type,image,power,drive,transmission,top_speed,zero_to_hundred,is_available,rent_price) values(@model,@production_year,@engine_type,@image,@power,@drive,@transmission,@top_speed,@zero_to_hundred,@is_available,@rent_price)", vehicle);
			}
		}

		public static List<string> getVehiclesEngines()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<string>("select name from engine_type");

				return output.ToList();
			}
		}

		public static void updateVehicle(CarModel car, int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<string>("UPDATE car SET model=@model,production_year=@production_year,engine_type=@engine_type,image=@image,power=@power,drive=@drive,transmission=@transmission,top_speed=@top_speed,zero_to_hundred=@zero_to_hundred,is_available=@is_available,rent_price=@rent_price WHERE id=" + id, car); 
			}
		}

		public static void deleteVehicle(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Execute("DELETE FROM car WHERE id=" + id);
			}
		}

        public static List<CarModel> getReservationVehicles(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
                var parameters = new { reservation_id = id };
                var output = cnn.Query<CarModel>("SELECT car.id,model,production_year,engine_type,image,power,drive,transmission,top_speed,zero_to_hundred,is_available,rent_price FROM car JOIN car__reservation ON car.id = car__reservation.car_id WHERE car__reservation.reservation_id=@reservation_id", parameters);

                return output.ToList();
            }
        }

		public static ClientModel getReservationClient(int id)
		{
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { reservation_id = id };
                var output = cnn.Query<ClientModel>("SELECT * FROM Client WHERE client.pesel = (SELECT reservation.client_pesel FROM reservation WHERE reservation.id = @reservation_id)", parameters);

				return output.FirstOrDefault();
            }
        }

		public static void DeleteReservation(int id)
		{
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { reservation_id = id };
				Console.WriteLine(parameters);
				var output = cnn.Execute("UPDATE reservation SET is_canceled=1 WHERE id=@reservation_id", parameters);
            }
        }
    }
}