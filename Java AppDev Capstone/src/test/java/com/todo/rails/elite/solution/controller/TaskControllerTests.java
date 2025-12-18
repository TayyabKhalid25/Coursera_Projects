package com.todo.rails.elite.solution.controller;

import com.todo.rails.elite.solution.model.Task;
import com.todo.rails.elite.solution.service.TaskService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.time.LocalDate;
import java.util.List;

import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

// TODO 13: Write Integration Tests for Controllers. Use MockMvc to test endpoints in TaskController.
@WebMvcTest(TaskController.class)
class TaskControllerTests {

	@Autowired
	private MockMvc mockMvc;

	@Mock
	private TaskService taskService;

	private Task sampleTask;

	@BeforeEach
	void setUp() {
		MockitoAnnotations.openMocks(this);
		sampleTask = new Task("Sample Task", "This is a sample task.", false, LocalDate.now());
	}

	@Test
	void getAllTasks_Success() throws Exception {
		when(taskService.getAllTasks()).thenReturn(List.of(sampleTask));

		mockMvc.perform(get("/api/tasks/all")
						.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk())
				.andExpect(jsonPath("$[0].title").value("Sample Task"));
	}
}
