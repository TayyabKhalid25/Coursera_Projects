package com.todo.rails.elite.solution.service;

import com.todo.rails.elite.solution.model.Task;
import com.todo.rails.elite.solution.repository.TaskRepository;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

// TODO 8: reformat code. Use your IDE's formatting tools to ensure consistent indentation and spacing.
// TODO 9: add method comments. Add method-level comments to explain the purpose and logic of methods.

/**
 * Service class for managing tasks in the application.
 *
 * <p>This class provides methods to perform operations on tasks, such as
 * adding, retrieving, updating, and deleting tasks. It interacts with the
 * {@link TaskRepository} to handle database operations and includes business
 * logic to ensure data integrity.</p>
 *
 * <h3>Key Features:</h3>
 * <ul>
 *   <li><strong>Add Task:</strong> Adds a new task to the database, ensuring no duplicate titles.</li>
 *   <li><strong>Retrieve Tasks:</strong> Fetch tasks by ID, title, or retrieve all tasks.
 *       Supports filtering tasks by completion status or due date.</li>
 *   <li><strong>Update Task:</strong> Updates an existing task's details, such as title, description,
 *       completion status, and due date.</li>
 *   <li><strong>Delete Task:</strong> Deletes a task from the database, ensuring it exists before deletion.</li>
 * </ul>
 *
 * <p><strong>Note:</strong> The methods use annotations to validate input parameters and
 * throw appropriate exceptions for invalid or missing data.</p>
 *
 * <h3>Usage:</h3>
 * <p>Inject this service into controllers or other services where task-related
 * operations are needed. Ensure proper exception handling for the methods.</p>
 */
@Service
public class TaskService {

	/**
	 * Repository for task-related database operations.
	 */
	private final TaskRepository taskRepository;

	// TODO 16: Log Exceptions. Use SLF4J to log exceptions in the service and controller layers.

	/**
	 * Constructor for TaskService.
	 *
	 * @param taskRepository the {@link TaskRepository} used for database operations.
	 */
	@Autowired
	public TaskService(TaskRepository taskRepository) {
		this.taskRepository = taskRepository;
	}

	/**
	 * Adds a new task to the database.
	 *
	 * @param task the {@link Task} object to be added.
	 * @return the saved {@link Task} object.
	 * @throws RuntimeException if a task with the same title already exists.
	 */
	public Task addTask(@NotNull(message = "Task cannot be null") Task task) throws RuntimeException {
		if (taskRepository.findByTitle(task.getTitle()).isPresent()) {
			throw new RuntimeException("Task already exists");
		}
		return taskRepository.save(task);
	}

	/**
	 * Retrieves a task by its ID.
	 *
	 * @param id the ID of the task to retrieve.
	 * @return the {@link Task} object with the specified ID.
	 * @throws RuntimeException if no task is found with the given ID.
	 */
	public Task getTaskById(@NotNull(message = "Id cannot be null") Long id) throws RuntimeException {
		return taskRepository.findById(id)
				.orElseThrow(
						() -> new RuntimeException("Task not found")
				);
	}

	/**
	 * Retrieves a task by its title.
	 *
	 * @param title the title of the task to retrieve.
	 * @return the {@link Task} object with the specified title.
	 * @throws RuntimeException if no task is found with the given title.
	 */
	public Task getTaskByTitle(
			@NotNull(message = "Title cannot be null")
			@NotBlank(message = "Title cannot be blank")
			String title
	) throws RuntimeException {
		return taskRepository.findByTitle(title)
				.orElseThrow(
						() -> new RuntimeException("Task not found")
				);
	}

	/**
	 * Retrieves all tasks from the database.
	 *
	 * @return a list of all {@link Task} objects. Returns an empty list if no tasks are found.
	 */
	public List<Task> getAllTasks() {
		if (taskRepository.findAll().isEmpty()) {
			return List.of();
		}
		return taskRepository.findAll();
	}

	/**
	 * Updates an existing task.
	 *
	 * @param task the {@link Task} object with updated details.
	 * @return the updated {@link Task} object.
	 * @throws RuntimeException if no task is found with the given title.
	 */
	public Task updateTask(@NotNull(message = "Task cannot be null") Task task) throws RuntimeException {
		// TODO 10: use meaningful names. Rename variables and methods for clarity. Ex - taskByTitle can be refactored to existingTask.
		Optional<Task> existingTask = taskRepository.findByTitle(task.getTitle());
		if (existingTask.isEmpty()) {
			throw new RuntimeException("Task not found");
		}
		Task taskToUpdate = existingTask.get();
		taskToUpdate.setTitle(task.getTitle());
		taskToUpdate.setDescription(task.getDescription());
		taskToUpdate.setCompleted(task.isCompleted());
		taskToUpdate.setDueDate(task.getDueDate());
		return taskRepository.save(taskToUpdate);
	}

	/**
	 * Deletes a task from the database.
	 *
	 * @param task the {@link Task} object to be deleted.
	 * @throws RuntimeException if no task is found with the given title.
	 */
	public void deleteTask(@NotNull(message = "Task cannot be null") Task task) throws RuntimeException {
		Optional<Task> taskByTitle = taskRepository.findByTitle(task.getTitle());
		if (taskByTitle.isEmpty()) {
			throw new RuntimeException("Task not found");
		}
		taskRepository.delete(task);
	}

	/**
	 * Retrieves all pending tasks (tasks not marked as completed).
	 *
	 * @return a list of pending {@link Task} objects. Returns an empty list if no pending tasks are found.
	 */
	public List<Task> getPendingTasks() {
		List<Task> allTasks = getAllTasks();
		if (allTasks.isEmpty()) {
			return List.of();
		}
		return allTasks.stream()
				.filter(task -> !task.isCompleted())
				.toList();
	}


	/**
	 * Retrieves all completed tasks.
	 *
	 * @return a list of completed {@link Task} objects. Returns an empty list if no completed tasks are found.
	 */
	public List<Task> getCompletedTasks() {
		List<Task> allTasks = getAllTasks();
		if (allTasks.isEmpty()) {
			return List.of();
		}
		return allTasks.stream()
				.filter(Task::isCompleted)
				.toList();
	}

	/**
	 * Retrieves all tasks due today.
	 *
	 * @return a list of {@link Task} objects due today. Returns an empty list if no tasks are due today.
	 */
	public List<Task> getTodayTasks() {
		List<Task> allTasks = getAllTasks();
		if (allTasks.isEmpty()) {
			return List.of();
		}
		return allTasks.stream()
				.filter(
						task -> !task.isCompleted()
				)
				.filter(
						task -> task.getDueDate()
								.isEqual(LocalDate.now())
				)
				.toList();
	}
}
