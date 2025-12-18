package com.todo.rails.elite.solution.exception;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

// TODO 15: Add Global Exception Handling. Use @ControllerAdvice and @ExceptionHandler to handle exceptions like EntityNotFoundException.
@ControllerAdvice
public class GlobalExceptionHandler {

	@ExceptionHandler(RuntimeException.class)
	public ResponseEntity<String> handleRuntimeException(RuntimeException ex) {
		return ResponseEntity.badRequest().body(ex.getMessage());
	}
}

