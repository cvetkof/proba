#include "stdafx.h"
#include <iostream>
#include <cstdlib>
#include <ctime>
using namespace std;

class zadachi
{
private:
	double t_nachal;
	double t_obrabotki;
	double vaznost;
	double udel_vaznost;
	int mesto;
public:
	int set_mesto(int l)
	{
		return mesto = l;
	}
	void get_udel_vaznost()
	{
		udel_vaznost = vaznost * vaznost / t_obrabotki;
	}
	//расчет удельной важности
	void get_parametrs()
	{
		cout << "начальное время - " << t_nachal << endl
			<< "время обработки - " << t_obrabotki << endl
			<< "важность - " << vaznost << endl
			<< "удельная важность - " << udel_vaznost << endl
			<< "место - " << mesto << endl;
	}
	//вывод параметров задачи на экран
	void set_rand_t_nachal()
	{
		t_nachal = rand() % 5400;
	}
	//ввод начального времени рандомно (0;5400)
	void set_rand_t_obrabotki()
	{
		t_obrabotki = rand() % 300;
	}
	//ввод времени обработки рандомно (0;300)
	void set_rand_vaznost()
	{
		vaznost = rand() % 100;
	}
	//ввод важности рандомно (0;100)
	
	void set_t_nachala()
	{
		cin >> t_nachal;
	}
	//ввод начального времени с руки
	void set_t_obrabotki()
	{
		cin >> t_obrabotki;
	}
	//ввод времени обработки с руки
	void set_vaznost()
	{
		cin >> vaznost;
	}
	//вводи важности с руки

	
	double znachenie_udel_vaznosti()
	{
		return udel_vaznost;
	}
	double znachenie_t_nachal()
	{
		return t_nachal;
	}
	double znachenie_t_obrabotki()
	{
		return t_obrabotki;
	}

	void pereprisvoenie_znacheniya_t_nachala(double t_nachal_last)
	{
		t_nachal = t_nachal_last;
	}
	// метод присваивания нового начального времени
	void pereprisvoenie_znacheniya_t_obrabotki(double t_obrabotki_last)
	{
		t_obrabotki += t_obrabotki_last;
	}
	// метод увеливающий суммарное время обработки

};


int main()
{
	setlocale(LC_ALL, "rus");
	srand (time (NULL));

	int kolichestvo_zadach;
	int directive_time = 5400;

	cout << "введите количество задач: ";
	cin >> kolichestvo_zadach;

	zadachi *zadacha = new zadachi[kolichestvo_zadach]; //выделение динамической памяти для массива задач
	zadachi *raspisanie = new zadachi[kolichestvo_zadach];
		
	//for (int i = 0;  i < kolichestvo_zadach; i++)	//
	//{												//
	//	zadacha[i].set_rand_t_nachal();				//
	//	zadacha[i].set_rand_t_obrabotki();			// инициальзация значений 
	//	zadacha[i].set_rand_vaznost();				// массива задач РАНДОМНО
	//	zadacha[i].get_udel_vaznost();				//
	//	zadacha[i].set_mesto(i+1);					//
	//}												//

	for (int i = 0; i < kolichestvo_zadach; i++)						//
	{																	//
		cout << "введите начальное время " << i+1 << "-ой задачи: ";	//
		zadacha[i].set_t_nachala();										// инициальзация значений
		cout << "введите время обратоки " << i + 1 << "-ой задачи: ";	// массива задач С РУКИ
		zadacha[i].set_t_obrabotki();									//
		cout << "введите важность " << i + 1 << "-ой задачи: ";			//
		zadacha[i].set_vaznost();										//
		zadacha[i].get_udel_vaznost();									//
	}

	/*for (int i = 0; i < kolichestvo_zadach; i++)
	{
		cout << "введите время обратоки " << i + 1 << "-ой задачи: ";
		zadacha[i].set_t_obrabotki();
	}*/

	for (int i = 0; i < kolichestvo_zadach; i++)				//
	{															//
		cout << "\nпараметры " << i+1 << "-ой задачи" << endl;	// вывод значений задач
		zadacha[i].get_parametrs();								// на экран
		cout << "--------------------------------------";		//
	}															//
	cout << endl;

	zadachi x;
	for (int i = 0; i < kolichestvo_zadach-1; i++)													//
		for (int j = 0; j < kolichestvo_zadach - i - 1; j++)										//
			if (zadacha[j].znachenie_udel_vaznosti() < zadacha[j + 1].znachenie_udel_vaznosti())	//
			{																						// сортировка по убыванию по удельной важности
				x = zadacha[j];																		//
				zadacha[j] = zadacha[j + 1];														//
				zadacha[j + 1] = x;																	//
			}																						//

	for (int i = 0; i < kolichestvo_zadach; i++)	// переназначение мест в соответствии с сортировкой
		zadacha[i].set_mesto(i + 1);				//

	cout << "========================================================" << endl;
	cout << "отсортированные по важнеости задачи" << endl;


	for (int i = 0; i < kolichestvo_zadach; i++)					//
	{																//
		cout << "\nпараметры " << i + 1 << "-ой задачи" << endl;	// вывод значений задач
		zadacha[i].get_parametrs();									// на экран в соответствии 
		cout << "--------------------------------------";			// в порядке сортировки
	}																//
	cout << endl;

	zadachi sovmesch_zadacha;
	for (int i = 0; i < kolichestvo_zadach; i++)
	{
		if (i == 0) sovmesch_zadacha = zadacha[0];
		else
		{
			for (int j = 0; j < i; j++)
			{
				if ((zadacha[i].znachenie_t_nachal() < (zadacha[j].znachenie_t_nachal() + zadacha[j].znachenie_t_obrabotki()))
					&& (zadacha[i].znachenie_t_nachal() > zadacha[j].znachenie_t_nachal()))
					i + 1;
				else
					if (zadacha[j].znachenie_t_nachal() == zadacha[i].znachenie_t_nachal())
						i + 1;
					else
						if (((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki()) > (zadacha[j].znachenie_t_nachal()))
							&& ((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki())
								< (zadacha[j].znachenie_t_nachal() + zadacha[j].znachenie_t_obrabotki())))
							i + 1;

				

				
			}
				

			//if ((zadacha[i].znachenie_t_nachal() < (zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()))	// условие, когда
			//	&& (zadacha[i].znachenie_t_nachal() > zadacha[i - 1].znachenie_t_nachal()))											// новая задача наезжает справа 
			//{																												 
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i].znachenie_t_obrabotki()); // увеличиваем время обработки объединенной задачи
			//	zadacha[i].pereprisvoenie_znacheniya_t_nachala(zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()); // присваение нового начального времени
			//}

			//if (zadacha[i].znachenie_t_nachal() == zadacha[i - 1].znachenie_t_nachal()) //условие когда начальное время задач совпадает
			//{
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i].znachenie_t_obrabotki()); // увеличиваем время обработки объединенной задачи
			//	zadacha[i].pereprisvoenie_znacheniya_t_nachala(zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()); // присваение нового начального времени
			//}

			//if (((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki()) > (zadacha[i - 1].znachenie_t_nachal())) // условие, когда
			//	&& ((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki())										 // новая задача наезжает слева
			//		< (zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki())))							 //
			//{
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i - 1].znachenie_t_obrabotki()); // увеличиваем время обработки объединенной задачи
			//	zadacha[i - 1].pereprisvoenie_znacheniya_t_nachala(zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki()); // присваение нового начального времени
			//} 		
		}
	}

	cout << "=======================================================================" << endl;
	cout << "составленное расписание:" << endl;
	for (int i = 0; i < kolichestvo_zadach; i++)
	{
		cout << "\nпараметры " << i + 1 << "-ой задачи" << endl;
		zadacha[i].get_parametrs();
		cout << "--------------------------------------";
	}

	cout << endl;


	//delete[] zadacha;
	return 0;
}