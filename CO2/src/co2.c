

#include <tizen.h>
#include <service_app.h> //defines a function prototype that used for handle service
#include <signal.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

/*#include "resource/resource_co2_sensor.h" //defines some variable, macro and data structure
#include "smartthings.h"
#include "smartthings_resource.h"
#include "smartthings_payload.h"*/
#include "log.h"
#include "thing_master_user.h"
#include "thing_resource_user.h"

extern pthread_mutex_t mutex;

#define UNUSED(x)		(void)(x)
#define MUTEX_LOCK		pthread_mutex_lock(&mutex);
#define MUTEX_UNLOCK	pthread_mutex_unlock(&mutex);

void handle_main_loop(void)
{
	pthread_t pthread[2];
	int ret = 0;

	signal(SIGSEGV, sig_handler);
	signal(SIGINT, sig_handler);
	signal(SIGTERM, sig_handler);

	pthread_mutex_init(&mutex, NULL);

	ret = pthread_create(&p_thread[0], NULL, &thread_sensor_main, NULL);
	if (ret != 0){
		_E("ERROR! thread_sensor_main create failed, ret=%d", ret);
		return;
	}
	ret = pthread_create(&p_thread[1], NULL, &thread_sensor_notify, NULL);
	if(ret !=0){
		_E("ERROR! thread_sensor_notify create failed, ret=%d", ret);
				return;
	}
}

void sig_handler(int sig)
{
	switch (sig)
	{
		case SIGINT:
			_I("SIGINT received");
			thread_done = 1;
			break;
		case SIGSEGV:
			_I("SIGSEGV received");
			thread_done = 1;
			break;
		case SIGTERM:
			_I("SIGTERM received");
			thread_done = 1;
			break;
		default:
			abort();
			break;
	}
}

static bool service_app_create(void *user_data)
{
	handle_main_loop();
	return true;
}

static void service_app_terminate(void *user_data)
{
	/*START;

	if (deinit_resource_app() != 0) {
		_E("deinit_resource_app failed");
	}

	if (deinit_master_app() != 0) {
		_E("deinit_master_app failed");
	}

	END;
	return;*/
	MUTEX_LOCK;
	thread_done = 1;
	MUTEX_UNLOCK;
	int staus = pthread_mutex_destroy(&mutex);
	_I("mutex destroy status = %d", status);
}

static void service_app_control(app_control_h app_control, void *user_data)
{
	START;

	if (app_control == NULL) {
		_E("app_control is NULL");
		return;
	}
	/*
	char *value = NULL;

	app_control_get_extra_data(app_control, "cmd", &value);
	_D("value: [%s]", value);
	if (value == NULL) {
		init_master_app();
		init_resource_app();
	} else
		_E("Unknown command");

	free(value);

	END;
	return;*/
	init_master_app();
	init_resource_app();
}

int main(int argc, char *argv[])
{
    char ad[50] = {0,};
	service_app_lifecycle_callback_s event_callback;

	event_callback.create = service_app_create;
	event_callback.terminate = service_app_terminate;
	event_callback.app_control = service_app_control;

	return service_app_main(argc, argv, &event_callback, NULL);
}

typedef struct {
	service_app_create_cb create; //This function gets called at the start of the application
	service_app_terminate_cb terminate; //This function gets called when the loop once exists
	service_app_control_cb app_control; //This function gets called when it gets the request to the application

};	service_app_lifecycle_callback_s;
