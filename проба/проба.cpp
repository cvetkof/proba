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
	//������ �������� ��������
	void get_parametrs()
	{
		cout << "��������� ����� - " << t_nachal << endl
			<< "����� ��������� - " << t_obrabotki << endl
			<< "�������� - " << vaznost << endl
			<< "�������� �������� - " << udel_vaznost << endl
			<< "����� - " << mesto << endl;
	}
	//����� ���������� ������ �� �����
	void set_rand_t_nachal()
	{
		t_nachal = rand() % 5400;
	}
	//���� ���������� ������� �������� (0;5400)
	void set_rand_t_obrabotki()
	{
		t_obrabotki = rand() % 300;
	}
	//���� ������� ��������� �������� (0;300)
	void set_rand_vaznost()
	{
		vaznost = rand() % 100;
	}
	//���� �������� �������� (0;100)
	
	void set_t_nachala()
	{
		cin >> t_nachal;
	}
	//���� ���������� ������� � ����
	void set_t_obrabotki()
	{
		cin >> t_obrabotki;
	}
	//���� ������� ��������� � ����
	void set_vaznost()
	{
		cin >> vaznost;
	}
	//����� �������� � ����

	
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
	// ����� ������������ ������ ���������� �������
	void pereprisvoenie_znacheniya_t_obrabotki(double t_obrabotki_last)
	{
		t_obrabotki += t_obrabotki_last;
	}
	// ����� ����������� ��������� ����� ���������

};


int main()
{
	setlocale(LC_ALL, "rus");
	srand (time (NULL));

	int kolichestvo_zadach;
	int directive_time = 5400;

	cout << "������� ���������� �����: ";
	cin >> kolichestvo_zadach;

	zadachi *zadacha = new zadachi[kolichestvo_zadach]; //��������� ������������ ������ ��� ������� �����
	zadachi *raspisanie = new zadachi[kolichestvo_zadach];
		
	//for (int i = 0;  i < kolichestvo_zadach; i++)	//
	//{												//
	//	zadacha[i].set_rand_t_nachal();				//
	//	zadacha[i].set_rand_t_obrabotki();			// ������������� �������� 
	//	zadacha[i].set_rand_vaznost();				// ������� ����� ��������
	//	zadacha[i].get_udel_vaznost();				//
	//	zadacha[i].set_mesto(i+1);					//
	//}												//

	for (int i = 0; i < kolichestvo_zadach; i++)						//
	{																	//
		cout << "������� ��������� ����� " << i+1 << "-�� ������: ";	//
		zadacha[i].set_t_nachala();										// ������������� ��������
		cout << "������� ����� �������� " << i + 1 << "-�� ������: ";	// ������� ����� � ����
		zadacha[i].set_t_obrabotki();									//
		cout << "������� �������� " << i + 1 << "-�� ������: ";			//
		zadacha[i].set_vaznost();										//
		zadacha[i].get_udel_vaznost();									//
	}

	/*for (int i = 0; i < kolichestvo_zadach; i++)
	{
		cout << "������� ����� �������� " << i + 1 << "-�� ������: ";
		zadacha[i].set_t_obrabotki();
	}*/

	for (int i = 0; i < kolichestvo_zadach; i++)				//
	{															//
		cout << "\n��������� " << i+1 << "-�� ������" << endl;	// ����� �������� �����
		zadacha[i].get_parametrs();								// �� �����
		cout << "--------------------------------------";		//
	}															//
	cout << endl;

	zadachi x;
	for (int i = 0; i < kolichestvo_zadach-1; i++)													//
		for (int j = 0; j < kolichestvo_zadach - i - 1; j++)										//
			if (zadacha[j].znachenie_udel_vaznosti() < zadacha[j + 1].znachenie_udel_vaznosti())	//
			{																						// ���������� �� �������� �� �������� ��������
				x = zadacha[j];																		//
				zadacha[j] = zadacha[j + 1];														//
				zadacha[j + 1] = x;																	//
			}																						//

	for (int i = 0; i < kolichestvo_zadach; i++)	// �������������� ���� � ������������ � �����������
		zadacha[i].set_mesto(i + 1);				//

	cout << "========================================================" << endl;
	cout << "��������������� �� ��������� ������" << endl;


	for (int i = 0; i < kolichestvo_zadach; i++)					//
	{																//
		cout << "\n��������� " << i + 1 << "-�� ������" << endl;	// ����� �������� �����
		zadacha[i].get_parametrs();									// �� ����� � ������������ 
		cout << "--------------------------------------";			// � ������� ����������
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
				

			//if ((zadacha[i].znachenie_t_nachal() < (zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()))	// �������, �����
			//	&& (zadacha[i].znachenie_t_nachal() > zadacha[i - 1].znachenie_t_nachal()))											// ����� ������ �������� ������ 
			//{																												 
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i].znachenie_t_obrabotki()); // ����������� ����� ��������� ������������ ������
			//	zadacha[i].pereprisvoenie_znacheniya_t_nachala(zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()); // ���������� ������ ���������� �������
			//}

			//if (zadacha[i].znachenie_t_nachal() == zadacha[i - 1].znachenie_t_nachal()) //������� ����� ��������� ����� ����� ���������
			//{
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i].znachenie_t_obrabotki()); // ����������� ����� ��������� ������������ ������
			//	zadacha[i].pereprisvoenie_znacheniya_t_nachala(zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki()); // ���������� ������ ���������� �������
			//}

			//if (((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki()) > (zadacha[i - 1].znachenie_t_nachal())) // �������, �����
			//	&& ((zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki())										 // ����� ������ �������� �����
			//		< (zadacha[i - 1].znachenie_t_nachal() + zadacha[i - 1].znachenie_t_obrabotki())))							 //
			//{
			//	sovmesch_zadacha.pereprisvoenie_znacheniya_t_obrabotki(zadacha[i - 1].znachenie_t_obrabotki()); // ����������� ����� ��������� ������������ ������
			//	zadacha[i - 1].pereprisvoenie_znacheniya_t_nachala(zadacha[i].znachenie_t_nachal() + zadacha[i].znachenie_t_obrabotki()); // ���������� ������ ���������� �������
			//} 		
		}
	}

	cout << "=======================================================================" << endl;
	cout << "������������ ����������:" << endl;
	for (int i = 0; i < kolichestvo_zadach; i++)
	{
		cout << "\n��������� " << i + 1 << "-�� ������" << endl;
		zadacha[i].get_parametrs();
		cout << "--------------------------------------";
	}

	cout << endl;


	//delete[] zadacha;
	return 0;
}